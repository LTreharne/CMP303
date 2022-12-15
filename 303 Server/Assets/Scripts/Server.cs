using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;


public class Server
{

    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;
    public static void Start(int maxPlayers, int port)
    {
        MaxPlayers = maxPlayers;
        Port = port;

        Debug.Log("Starting server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(UDPRecieveCallBack, null);

        Debug.Log($"Server started on port {Port}.");
    }

    private static void TCPConnectCallback(IAsyncResult result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Debug.Log($"Incoming connection from {client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                clients[i].tcp.Connect(client);
                return;
            }
        }

        Debug.Log($"{client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPRecieveCallBack(IAsyncResult result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(result, ref clientEndPoint);
            udpListener.BeginReceive(UDPRecieveCallBack, null);

            if (data.Length < 4)
            {
                return;
            }
            using (Packet pack = new Packet(data))
            {
                int clientID = pack.ReadInt();
                if (clientID == 0)
                {
                    return;
                }
                if (clients[clientID].udp.endPoint == null)
                {
                    clients[clientID].udp.Connect(clientEndPoint);
                    return;
                }
                if (clients[clientID].udp.endPoint.ToString() == clientEndPoint.ToString())
                {
                    clients[clientID].udp.HandleData(pack);
                }


            }

        }
        catch (Exception ex)
        {
            Debug.Log($"Error Recieving UDP Data {ex}");
        }
    }
    public static void SendUDPData(IPEndPoint clientEndpoint, Packet packet)
    {
        try
        {
            if (clientEndpoint != null)
            {
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndpoint, null, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"ERROR SENDING UDP DATA {ex}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived, ServerHandler.WelcomeReceived },
                { (int)ClientPackets.playerMovement, ServerHandler.PlayerMovement },
                { (int)ClientPackets.playerShoot, ServerHandler.PlayerShoot },
            };
        Debug.Log("Initialized packets.");
    }

    public static void Stop()
    {
        tcpListener.Stop();
        udpListener.Close();
    }
}
