using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public static Dictionary<int, HealthPackSpawner> healthPacks = new Dictionary<int, HealthPackSpawner>();
    private static int nextSpawnerID = 1;

    public int spawnerID;
    public bool spawned = false;

    private void Start()
    {
        spawned = false;
        spawnerID = nextSpawnerID;
        nextSpawnerID++;
        healthPacks.Add(spawnerID, this);

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
        ServerSend.HealthPackRespawn(spawnerID);
    }

    private void Collected(int byPlayer)
    {
        spawned = false;
        ServerSend.HealthPackColelcted(spawnerID, byPlayer);
        StartCoroutine(SpawnPack());
    }
}
