using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//manages the packets recieved from clietns which corrilate to movement, shooting and connection confirmaton 
public class ServerHandler
{
    public static void WelcomeReceived(int fromClient, Packet packet)
    {
        int clientIdCheck = packet.ReadInt();
        string username = packet.ReadString();

        if (fromClient != clientIdCheck)
        {
            Debug.Log($"Player \"{username}\" (ID: {fromClient}) has assumed the wrong client ID ({clientIdCheck})!");
        }
        // TODO: send player into game
        Server.clients[fromClient].SendIntoGame(username);
    }

    public static void PlayerMovement(int fromClient, Packet packet)
    {
        bool[] inputs = new bool[packet.ReadInt()];

        for (int i = 0; i < inputs.Length; ++i)
        {
            inputs[i] = packet.ReadBool();
        }
        Quaternion rotation = packet.ReadQuaternion();

        Server.clients[fromClient].player.SetInput(inputs, rotation);
    }

    public static void PlayerShoot(int fromClient, Packet packet)
    {
        Vector3 shootDirection = packet.ReadVector3();

        Server.clients[fromClient].player.Shoot(shootDirection);
    }


}
