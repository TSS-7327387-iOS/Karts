using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    EnemyAI enemyAI;
    Character character;
    public bool isPlayer,isBot;
    Collider col;
    public int damage,radius;
    [SerializeField]    public Transform rayTarget, detectTarget; //Setit from chase target

    //public Transform target;
    private void OnEnable()
    {
        if (enemyAI == null && !isPlayer && !isBot)
        { 
            enemyAI = GetComponentInParent<EnemyAI>();
            enemyAI.detectPlayer = this;
        }
        
        if(character == null) character = GetComponentInParent<Character>();

        if(col==null)  col = GetComponent<Collider>();

        //MyChange

        ////isOn = true;
        ////InvertState();

        if (!isBot)
        {
            isOn = true;
            InvertState();
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        

        if (other.transform==character.transform)
            return;
        ////if (!isBot && !isPlayer && other.CompareTag("Player"))
        ////{
        ////    //if (GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured && enemyAI.move.ChasingPlayer)
        ////    //    enemyAI.player.playerGotTouched(transform.position - other.transform.position);

        ////}
        ////else if (!isBot && other.CompareTag("Enemy"))
        ////{
        ////    //print("----------------Enemy Touch---------------");

        ////    ////if (!GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured)
        ////    ////    other.GetComponent<EnemyAI>().DropBall(transform.position - other.transform.position);

        ////}
        ////else 
        


        //----------------------------------------------------------------------------------------------------------------------------------




        //////////////if (other.TryGetComponent<Character>(out Character cc))
        //////////////{
        //////////////    if (isBot && cc.isBot) //Bot to Bot
        //////////////        return;

        //////////////    if (!isPlayer && !isBot && cc.isEnemy) //Enemy to enemy
        //////////////        return;

        //////////////    cc.TakeDamage(damage);

        //////////////}

        
        //////////////if (!isBot && other.CompareTag("Pickup"))
        //////////////{
        //////////////    other.GetComponent<Pickable>().TakeDemage(damage);
        //////////////}




        //////////////if (isPlayer & other.CompareTag("Bot"))
        //////////////{
        //////////////    if (other.GetComponent<BotAI>().health <= 0f)
        //////////////    {
        //////////////        print("Refill giant 2");
        //////////////        if (other.GetComponent<BotAI>().isGiant)
        //////////////        {
        //////////////            GameManager.Instance.RefillPowerGiant(transform.position + new Vector3(0f, 0.5f, 1f));
        //////////////            print("Refill giant");
        //////////////        }
        //////////////        else
        //////////////        {
        //////////////            GameManager.Instance.RefillPower(transform.position + new Vector3(0f, 0.5f, 1f));
        //////////////            print("Refill");
        //////////////        }

        //////////////    }
        //////////////}
    }

    //----------------------------------------------------------------------------------------------------------------------------------

    //private void OnTriggerExit(Collider other)
    //{
    //    if (isPlayer & other.CompareTag("Bot"))
    //    {
    //        print("Bot Trigger Exit  :  " + other.GetComponent<BotAI>().health);
    //        if (other.GetComponent<BotAI>().health <= 0f)
    //        {
    //            print("Refill giant 2");
    //            if (other.GetComponent<BotAI>().isGiant)
    //            {
    //                GameManager.Instance.RefillPowerGiant(transform.position + new Vector3(0f, 0.5f, 1f));
    //                print("Refill giant");
    //            }
    //            else
    //            {
    //                GameManager.Instance.RefillPower(transform.position + new Vector3(0f, 0.5f, 1f));
    //                print("Refill");
    //            }

    //        }
    //    }
    //}

    bool isOn;
    public void InvertState()
    {
        isOn = !isOn;
        col.enabled = isOn;
        col.isTrigger = isOn;
    }
    public void SetCollider(bool val) //Player Function
    {
        col.enabled = val;
        col.isTrigger = val;
    }

    private void OnDisable()
    {
        if (isBot)
        {
            col.enabled = false;
        }
    }





    public float rayDistance = 10f; // Adjust the range
    public LayerMask detectionLayer; // Assign relevant layers in Inspector

    public bool IsActiveDemageRaycast;

  /* private void Start()
    {
        rayDistance = 6;
        //rayTarget = transform.Find("RayOrigin");
    }*/


   /* private void Update()
    {
        if (IsActiveDemageRaycast)
        {
            DetectAndAct();
            IsActiveDemageRaycast = false;
        }
    }*/
    //RaycastHit[] hits = new RaycastHit[5]; // Preallocated array (adjust size if needed)
    Collider[] colliders = new Collider[9];
    /*void DelayDetect()
    {
        CancelInvoke();
        DetectAndAct();
    }*/
    public void DetectAndAct()
    {
        print("Method called");
        if (rayTarget == null)
        {
            Debug.Log(gameObject);
            return;
        }
        print("Has raytarget");

        

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }

        //if (Physics.RaycastNonAlloc(character.transform.position+Vector3.up, character.transform.forward, hits, rayDistance,detectionLayer, QueryTriggerInteraction.Collide) > 0)
        if (Physics.OverlapSphereNonAlloc(rayTarget.position, 0.75f, colliders, detectionLayer) > 0)
        {
            // Cast a ray forward from the object's position
            //foreach (var hit in hits)
            foreach (var hit in colliders)
            {
                //if (hit.collider == null)
                if (hit == null)
                    continue;
                //Collider other = hit.collider;
                Collider other = hit;
                   // Debug.LogError(character._name+ " Ray hitted on: "+hit.name);
                if (!isBot && detectTarget != null)
                {
                    if (other.transform == character.transform &&  detectTarget != other.transform)
                        continue;
                }
                else if (other.transform == character.transform)
                {
                    continue; 
                }


                if (other.TryGetComponent<Character>(out Character cc))
                {
                    if (isBot && cc.isBot) // Bot to Bot
                        return;

                    if (!isPlayer && !isBot && cc.isEnemy) // Enemy to enemy
                        return;

                    cc.TakeDamage(damage);
                }

                if (!isBot && other.CompareTag("Pickup"))
                {
                    other.GetComponent<Pickable>().TakeDemage(damage);
                    return;
                }

                if (isPlayer && other.CompareTag("Bot"))
                {
                    BotAI botAI = other.GetComponent<BotAI>();

                    if (botAI.health <= 0f && !botAI.IsChest)
                    {
                        print("Refill giant 2");

                        if (botAI.isGiant)
                        {
                            GameManager.Instance.RefillPowerGiant(transform.position + new Vector3(0f, 0.5f, 1f));
                            print("Refill giant");
                        }
                        else
                        {
                            GameManager.Instance.RefillPower(transform.position + new Vector3(0f, 0.5f, 1f));
                            print("Refill");
                        }
                    }
                }

            }
        }
        //else if (character.isBot) Debug.DrawRay(character.transform.position + Vector3.up, character.transform.forward, Color.red, rayDistance);

    }
/*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(rayTarget.position, 1);
    }
*/
}
