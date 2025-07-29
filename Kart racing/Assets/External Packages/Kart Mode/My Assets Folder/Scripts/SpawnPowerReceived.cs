using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPowerReceived : MonoBehaviour
{
    public float health;
    private float startHealth;

    private void Start()
    {
        startHealth = health;
    }

    public void AddHealth(float _health)
    {
        //If health less than max health
        if(health < startHealth)
            health += _health;

        //If health exceed from max health
        if(health > startHealth)
            health = startHealth;
    }

    public void GiveDemage(float demage)
    {
        //Give demage if health more than zero
        if (health > 0f)
             health -= demage;
    }
}
