using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//initialised Sniper. checks if players collide with the objects manages altering player Damage amount, depsawning and ensures the sniper ont respawn

public class SniperSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<int, SniperSpawner> sniperSpawns = new Dictionary<int, SniperSpawner>();
    private static int nextSpawnerID = 1;

    public int spawnerID;
    public bool spawned = false;

    private void Start()
    {
        spawned = false;
        spawnerID = nextSpawnerID;
        nextSpawnerID++;
        sniperSpawns.Add(spawnerID, this);

        StartCoroutine(SpawnPack());
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        Collected(player.id);

    }
    private IEnumerator SpawnPack()
    {
        yield return new WaitForSeconds(10.0f);

        spawned = true;
        ServerSend.SniperRespawn(spawnerID);
    }

    private void Collected(int byPlayer)
    {
        spawned = false;
        Server.clients[byPlayer].player.playerDamage = 100.0f;
        ServerSend.SniperColelcted(spawnerID, byPlayer);
    }
}
