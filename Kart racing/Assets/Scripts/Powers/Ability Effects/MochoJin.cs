using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MochoJin : Powers
{
    public Powers[] powers;

    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        if (character.isEnemy)
        {
        }
    }

    public override void MochoJinAttack(float r,float d)
    {

    }


}
