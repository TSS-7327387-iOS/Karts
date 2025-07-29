using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minions : MonoBehaviour
{
    public Character parent;
    public NavMeshAgent agent;
    public Animator animator;
    public MinionDetector detector;
    EnemyAI enemyAI;
    Player player;
    bool isActive;
    public float followParentDistance;
    public void InitializedMinion(Character character)
    {
        parent = character;
        detector.chhh = parent;
        isActive = true;
       // if (parent.isEnemy)
       // {
            enemyAI = parent.GetComponent<EnemyAI>();
/*        }
        else
        {
            player = parent.GetComponent<Player>();
        }*/
    }
    
    bool canFight,fight;

    void Update()
    {
        if (isActive)
        {
            canFight = Vector3.Distance(transform.position,parent.transform.position) <= followParentDistance;
            if(!fight||!canFight) agent.SetDestination(parent.transform.position+parent.transform.forward);
            animator.SetBool("Move", agent.velocity.magnitude > 0.1f);
            animator.SetBool("Fight", fight && canFight);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.TryGetComponent<Character>(out Character charac))
        {
            if (charac == parent) return;
                fight = charac.isAlive;
            if (fight && canFight) agent.SetDestination(charac.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character charac))
        {
            if (charac == parent) return;
                fight = false;
        }
    }
}
