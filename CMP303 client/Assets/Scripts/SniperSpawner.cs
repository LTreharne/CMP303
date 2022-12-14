using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public bool hasItem;
    public MeshRenderer model;

    private Vector3 spawnPoint;

    public void Initialise(int spawnID, bool hasITEM)
    {
        id = spawnID;
        hasItem = hasITEM;
        model.enabled = hasItem;

        spawnPoint = transform.position;
    }

    public void Respawn()
    {
        hasItem = true;
        model.enabled = true;
    }
    public void Collect()
    {
        hasItem = false;
        model.enabled = false;
    }
}
