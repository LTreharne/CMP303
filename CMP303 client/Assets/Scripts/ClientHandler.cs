using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

//USED to handle packets recieved from the server each packet has its own enum telling the server what type of packet its dealing with and will call the correct function based off the clients packet dictonary
public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet packet) {

        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector3 positon = packet.ReadVector3();
        Quaternion rotation = packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(id, username, positon, rotation);
    }

    public static void PlayerPosition(Packet packet) {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();

        if (id == Client.instance.myId)//if the id is = clients id disables input prediction for 1 frame to avoid the rubber banding issue discussed in playerManager .cs comment
        {
                GameManager.players[id].transform.position = position;
                PlayerController player = FindObjectOfType<PlayerController>();
                player.prediction = false;
            
        }
        else { 
            GameManager.players[id].transform.position = position;
        }

    }  
    
    public static void PlayerRotation(Packet packet) {
        int id = packet.ReadInt();

        Quaternion rotation = packet.ReadQuaternion();

        GameManager.players[id].transform.rotation = rotation;
    
    }

    public static void PlayerDisconnected(Packet packet)
    {
        int id = packet.ReadInt();
        Destroy(GameManager.players[id].gameObject);
        GameManager.players.Remove(id);

    }

    public static void PlayerHealth(Packet packet)
    {
        int id = packet.ReadInt();
        float health = packet.ReadFloat();

        GameManager.players[id].setHeath(health);
    }

    public static void PlayerRespawn(Packet packet)
    {
        int id = packet.ReadInt();

        GameManager.players[id].Respawn();
    }

   public static void CreateHealthPackSpanwer(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        bool hasItem = packet.ReadBool();

        GameManager.instance.CreateItemSpanwer(id, position, hasItem);
    }

    public static void HealthPackRespawn(Packet packet)
    {
        int id = packet.ReadInt();
        GameManager.healthPacks[id].Respawn();
    } 
    
    public static void HealthPackCollected(Packet packet)
    {
        int id = packet.ReadInt();
        int playerID = packet.ReadInt();
        GameManager.healthPacks[id].Collect();
        GameManager.players[playerID].health = GameManager.players[playerID].maxHealth;
    }
    
    public static void CreateSniperSpanwer(Packet packet)
    {
        int id = packet.ReadInt();
        Vector3 position = packet.ReadVector3();
        bool hasItem = packet.ReadBool();

        GameManager.instance.CreateSniperSpawner(id, position, hasItem);
    }

    public static void SniperRespawn(Packet packet)
    {
        int id = packet.ReadInt();
        GameManager.SniperSpawns[id].Respawn();
    } 
    
    public static void SniperCollected(Packet packet)
    {
        int id = packet.ReadInt();
        int playerID = packet.ReadInt();
        GameManager.SniperSpawns[id].Collect();
    }
}