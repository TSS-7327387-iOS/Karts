using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnPowerGiven : MonoBehaviour
{
    public enum PowerAttribute
    {
        Health,Trap,Bullet,Missile,Nos
    }
    
    [SerializeField] private PowerAttribute selectedPower;
    
    [SerializeField, HideInInspector] private int healthAmount;
    [SerializeField, HideInInspector] private int trapDamage;
    [SerializeField, HideInInspector] private int bulletDemage;
    [SerializeField, HideInInspector] private int missileDamage;
    //[SerializeField, HideInInspector] private float nosBoostAmount;
    
    [SerializeField, HideInInspector] private GameObject powerReceivedVfx;
    
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") || collision.transform.CompareTag("Enemy"))
        {
            SpawnPowerReceived spawnPowerReceived = collision.transform.GetComponentInParent<SpawnPowerReceived>();
    
            if (powerReceivedVfx)
            {
                Instantiate(powerReceivedVfx, collision.transform.position, powerReceivedVfx.transform.rotation, collision.transform);
    
            }
    
            switch (selectedPower)
            {
                case PowerAttribute.Health:
                    spawnPowerReceived.AddHealth(healthAmount);
                    break;
    
                case PowerAttribute.Trap:
                    spawnPowerReceived.GiveDemage(trapDamage);
                    break;
    
                case PowerAttribute.Bullet:
                    spawnPowerReceived.GiveDemage(bulletDemage);
                    break;
    
                case PowerAttribute.Missile:
                    spawnPowerReceived.GiveDemage(missileDamage);
                    break;
            }
            
        }
    }
}
    
    
