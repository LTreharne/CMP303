using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
    public Transform[] spawnPoints;
    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;


        Server.Start(4, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
        else if (instance !=this)
        {
            Debug.Log("Funky stuff happenin");
            Destroy(this);
        
        }   
    }

    public Player InstanciatePlayer(int id)
    {
        return Instantiate(playerPrefab, spawnPoints[id].transform.position, Quaternion.identity).GetComponent<Player>();
    }
}
