using System;
using UnityEngine;

public class HealthSnatcher : Powers
{
    public int damage;
    private void Start()
    {
        base.Initialize();
    }
    public override void HealthSnatch(float radius)
    {

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Character>(out Character ch))
            {
                if (ch != character)
                {
                    ch.TakeDamage(damage);
                }
            }

        }
        PlayPowerSound();
    }

    
}
