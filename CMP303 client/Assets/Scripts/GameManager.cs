using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, HealthPackSpawner> healthPacks = new Dictionary<int, HealthPackSpawner>();   
    public static Dictionary<int, SniperSpawner> SniperSpawns = new Dictionary<int, SniperSpawner>();

    public int localPlayerID;
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject healthPrefab;
    public GameObject SniperPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation) {
        GameObject player;
        if (id == Client.instance.myId)
        {
            player = Instantiate(localPlayerPrefab, position, rotation);
        }
        else 
        {
            player = Instantiate(playerPrefab, position, rotation);
        }

        player.GetComponent<PlayerManager>().Initialise(id, username);
        players.Add(id, player.GetComponent<PlayerManager>());
    }

    public void CreateItemSpanwer(int id, Vector3 position, bool hasItem)
    {
        GameObject spawner = Instantiate(healthPrefab, position, healthPrefab.transform.rotation);

        spawner.GetComponent<HealthPackSpawner>().Initialise(id, hasItem);
        healthPacks.Add(id, spawner.GetComponent<HealthPackSpawner>());
    } 
    
    public void CreateSniperSpawner(int id, Vector3 position, bool hasItem)
    {
        GameObject spawner = Instantiate(SniperPrefab, position, SniperPrefab.transform.rotation);

        spawner.GetComponent<SniperSpawner>().Initialise(id, hasItem);
        SniperSpawns.Add(id, spawner.GetComponent<SniperSpawner>());
    }
}
