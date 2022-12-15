using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//THis class maanges things related to the game in unity
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    //Dictonary to store players, healthpacks and sniper spawns
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, HealthPackSpawner> healthPacks = new Dictionary<int, HealthPackSpawner>();   
    public static Dictionary<int, SniperSpawner> SniperSpawns = new Dictionary<int, SniperSpawner>();

    //Prefab models for healthpacks, players and snipers
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
            //Destroys the instance of game manager if one already exists
            Destroy(this);
        }
    }

    public void SpawnPlayer(int id, string username, Vector3 position, Quaternion rotation) {//spawns player using data recieved from the server
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

    //CREATE item spawners using data from the server
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
