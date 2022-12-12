using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public MeshRenderer mesh;

    public void Initialise(int ID, string USERNAME)
    {
        username = USERNAME;
        id = ID;
        health = maxHealth;
    }

    public void setHeath(float heal)
    {
        health = heal;

        if (health <=0)
        {
            Die();
        }
    }

    public void Die()
    {
        mesh.enabled = false;
    }

    public void Respawn()
    {
        mesh.enabled = true;
        setHeath(maxHealth);
    }
}
