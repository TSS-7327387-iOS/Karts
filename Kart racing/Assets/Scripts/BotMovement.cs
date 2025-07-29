using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotMovement : AImovement
{
    [SerializeField] float radiusToWander;
    Transform target;
    public bool chase;
    Rigidbody rb;

    private void OnEnable()
    {
        chase = false;
        botAI = GetComponent<BotAI>();
        if (rb == null) rb = GetComponent<Rigidbody>();
    }
    int attackMeter=0,attackMetterTemp=0;

    // Update is called once per frame
    void Update()
    {
        if (botAI.isAlive)
        {
            if (!chase)
            {
                agent.speed = runningSpeed;
                if (agent.remainingDistance <= agent.stoppingDistance || !agent.hasPath)
                    agent.SetDestination(GetRandomPoint());
            }
            else
            {
                if (botAI.target == null)
                    return;

                if (Vector3.Distance(transform.position, botAI.target.transform.position) < stoppingDistance)
                {
                    transform.LookAt(botAI.target.transform);
                    animator.SetBool("Chase", false);
                    attackMeter = 1;
                    if (attackMeter != attackMetterTemp)
                    {
                        attackMetterTemp = attackMeter;
                        animator.SetInteger("Attack",attackMeter);
                    }
                    
                    agent.ResetPath(); agent.speed = 0;

                    ////print("Bot Attack : "+transform.name);

                    return;
                }
                agent.speed = chasingSpeed;
                attackMeter = 0;
                if (attackMeter != attackMetterTemp)
                {
                    attackMetterTemp = attackMeter;
                    animator.SetInteger("Attack", attackMeter);
                }
                agent.SetDestination(botAI.target.position);
                animator.SetBool("Chase", rb.velocity.magnitude > 0);
            }
        }
    }
    
    public void PlayDead()
    {
        agent.speed = 0;
        agent.ResetPath();
        animator.SetTrigger("Dead");
     //   animator.SetBool("Wander", false);
        animator.SetBool("Chase", false);
        animator.SetInteger("Attack", 0);
    }
    public override void changeStateToRun() //Wandering
    {
       // animator.SetBool("Wander", true);
        animator.SetBool("Chase", false);
        animator.SetInteger("Attack", 0);
        chase =false;

    }
    public override void ChangeStateToChase()
    {
        chase=true;
        agent.speed = chasingSpeed;
       // animator.SetBool("Wander", false);
        animator.SetInteger("Attack", 0);
    }
    public Vector3 GetRandomPoint()
    {

        Vector3 randomDirection = Random.insideUnitSphere * radiusToWander;
        randomDirection += transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, radiusToWander, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }

}
