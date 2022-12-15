using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Both this class and The health pack spawner are similar managing item spawners. they are dont in different classes as they have different repsawn times/requiremnts...
//...and thusly are called via different packets

public class HealthPackSpawner : MonoBehaviour
{
    public int id;
    public bool hasItem;
    public MeshRenderer model;

    private Vector3 spawnPoint;

    public void Initialise(int spawnID, bool hasITEM)//setus up spawner with data recieved from server
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
