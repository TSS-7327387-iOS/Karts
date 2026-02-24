using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AImovement:MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public float chasingSpeed, runningSpeed, stoppingDistance, powerRange;
    public EnemyAI enemy;
    public BotAI botAI;
    public virtual void changeStateToRun()
    {

    }
    public virtual void ChangeStateToChase()
    {

    }
   
}
