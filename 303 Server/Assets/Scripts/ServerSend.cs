using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Sends data to clients 
public class ServerSend
{
    //TCP SEND FUNCTIONS
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendTCPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }
    private static void SendTCPDataToAllBarOne(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }

    //UDP SEND FUNCTIONS
    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }


    private static void SendUDPDataToAll(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }
    private static void SendUDPDataToAllBarOne(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != exceptClient)
            {
                Server.clients[i].udp.SendData(packet);
            }
        }
    }


    //ACTUAL SENDING STUFF
    public static void Welcome(int toClient, string msg)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcome))
        {
            packet.Write(msg);
            packet.Write(toClient);

            SendTCPData(toClient, packet);
        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.username);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);

            SendTCPData(toClient, packet);
        }
    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);

            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerRotation(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerRotation))
        {
            packet.Write(player.id);
            packet.Write(player.transform.rotation);

            SendUDPDataToAllBarOne(player.id, packet);
        }
    }

    public static void PlayerDisconnected(int pID)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            packet.Write(pID);
            

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerHealth(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerHealth))
        {
            packet.Write(player.id);
            packet.Write(player.health);

            SendTCPDataToAll(packet);
        }
    }

    public static void PlayerRespawn(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerRespawn))
        {
            packet.Write(player.id);

            SendTCPDataToAll(packet);
        }
    }

    public static void CreateItemSpawner(int toClient, int spawnerID, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet((int)ServerPackets.createHealthPackSpawner))
        {
            packet.Write(spawnerID);
            packet.Write(position);
            packet.Write(hasItem);

            SendTCPData(toClient, packet);
        }
    }

    public static void HealthPackRespawn(int id)
    {
        using (Packet packet = new Packet((int)ServerPackets.healthpackRespawn))
        {
            packet.Write(id);

            SendTCPDataToAll(packet);
        }
    }

    public static void HealthPackColelcted(int id, int byPlayer)
    {
        using (Packet packet = new Packet((int)ServerPackets.healthPackCollected))
        {
            packet.Write(id);
            packet.Write(byPlayer);
            SendTCPDataToAll(packet);
        }
    }

    public static void SniperColelcted(int id, int byPlayer)
    {
        using (Packet packet = new Packet((int)ServerPackets.SniperCollected))
        {
            packet.Write(id);
            packet.Write(byPlayer);
            
            SendTCPDataToAll(packet);
        }
    }

    public static void SniperRespawn(int id)
    {
        using (Packet packet = new Packet((int)ServerPackets.sniperRespawn))
        {
            packet.Write(id);

            SendTCPDataToAll(packet);
        }
    }
    public static void CreateSniperSpawner(int toClient, int spawnerID, Vector3 position, bool hasItem)
    {
        using (Packet packet = new Packet((int)ServerPackets.CreateSniperSpawner))
        {
            packet.Write(spawnerID);
            packet.Write(position);
            packet.Write(hasItem);

            SendTCPData(toClient, packet);
        }
    }

}
