using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FirePunch : Powers
{
    public ProjectileMover fireBall;
    public Transform shootPoint;
    private void Start()
    {
        base.Initialize();
    }
    public override void FireAttack(Transform target)
    {
        if (character.isEnemy)
        {
            shootPoint.LookAt(target);
           // Instantiate(fireBall, shootPoint.position, shootPoint.rotation).InitializeMissile(character);
        }
        else
        {
           // Instantiate(fireBall, shootPoint.position, character.transform.rotation).InitializeMissile(character);
        }
        character.isAnimatingPower = false;
        PlayPowerSound();
    }

    
}
