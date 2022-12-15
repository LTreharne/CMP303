using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;

//CLIENT CLASS IS IDENTICAL to the one in client project
public class Client
{
    public static int dataBufferSize = 4096;

    public int id;
    public Player player;
    public TCP tcp;
    public UDP udp;

    public Client(int _clientId)
    {
        id = _clientId;
        tcp = new TCP(id);
        udp = new UDP(id);
    }

    public class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = dataBufferSize;
            socket.SendBufferSize = dataBufferSize;

            stream = socket.GetStream();

            receivedData = new Packet();
            receiveBuffer = new byte[dataBufferSize];

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);

            ServerSend.Welcome(id, "Welcome to the server!");
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error sending data to player {id} via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Server.clients[id].Disconnect();
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception _ex)
            {
                Debug.Log($"Error receiving TCP data: {_ex}");
                Server.clients[id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }

            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        Server.packetHandlers[_packetId](id, _packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        public void Disconenct()
        {
            socket.Close();
            stream = null;
            receiveBuffer = null;
            receivedData = null;
            socket = null;
        }
    }

    public class UDP
    {
        public IPEndPoint endPoint;

        private int id;

        public UDP(int ID)
        {
            id = ID;
        }

        public void Connect(IPEndPoint endP)
        {
            endPoint = endP;
        }

        public void SendData(Packet packet)
        {
            Server.SendUDPData(endPoint, packet);
        }

        public void HandleData(Packet packet)
        {
            int packetLength = packet.ReadInt();
            byte[] packetBytes = packet.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet pack = new Packet(packetBytes))
                {
                    int packetID = pack.ReadInt();
                    Server.packetHandlers[packetID](id, pack);
                }
            });
        }

        public void Disconnect()
        {
            endPoint = null;
        }
    }


    public void SendIntoGame(string name)
    {
        player = NetworkManager.instance.InstanciatePlayer(id);
        player.Initialize(id, name);
        

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                if (client.id != id)
                {
                    ServerSend.SpawnPlayer(id, client.player);
                }
            }
        }

        foreach (Client client in Server.clients.Values)
        {
            if (client.player != null)
            {
                ServerSend.SpawnPlayer(client.id, player);
            }
        }

        foreach (HealthPackSpawner healthPacks in HealthPackSpawner.healthPacks.Values) 
        {
            ServerSend.CreateItemSpawner(id, healthPacks.spawnerID, healthPacks.transform.position, healthPacks.spawned);
        }

        foreach (SniperSpawner Snipers in SniperSpawner.sniperSpawns.Values)
        {
            ServerSend.CreateSniperSpawner(id, Snipers.spawnerID, Snipers.transform.position, Snipers.spawned);
        }
    }

    private void Disconnect()
    {
        Debug.Log($"{tcp.socket.Client.RemoteEndPoint} has been disconnected");

        ThreadManager.ExecuteOnMainThread(() => { 
            UnityEngine.Object.Destroy(player.gameObject);
        });

        tcp.Disconenct();
        udp.Disconnect();

        ServerSend.PlayerDisconnected(id);
    }

}

