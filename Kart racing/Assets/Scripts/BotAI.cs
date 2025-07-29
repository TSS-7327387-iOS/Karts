using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BotAI : Character
{
    public int  attackDamage;
    public Transform target;
    public List<Transform> nearbyAgents=new List<Transform>();
    BotMovement move;
    public float stoppingDistanceForOthers;
    //public bool isAlive;
    public GameObject deathEffect;
    public AudioSource deathAudioSource;
    public bool isGiant;

    int healthActual;


    //MyChange
    public bool IsChest;




    private void Awake()
    {
         InitializeBotAI();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (move == null)
            move = GetComponent<BotMovement>();
        isAlive = true;

        healthActual = health;
        
    }

    private void Update()
    {
        if(target != null)
        {
            if (target.TryGetComponent<Character>(out Character charac))
            {
                if (charac.isBot)
                    return;

                if(!charac.isAlive)
                {
                    nearbyAgents.Remove(target);
                    if (nearbyAgents.Count == 0)
                    {
                        ChangeStateToWander();
                    }
                    else
                    {
                        ChaseTarget();
                    }
                }
            }
        }
    }


    public void ChaseTarget()
    {
        if (nearbyAgents.Count == 0)
            return;
        target = nearbyAgents[0]; 
        move.ChangeStateToChase();
    }
    public void ChangeStateToWander()
    {
        target = null;
        move.changeStateToRun();
    }
    public void CheckForEnemyTargetDeath(Transform _target)
    {
        for(int i = 0; i < nearbyAgents.Count; i++)
        {
            if (_target == nearbyAgents[i])
            {
                nearbyAgents.RemoveAt(i);
                break;
            }
        }
        ChaseTarget();
    }
    public override void Die()
    {
        //////if (!IsChest)
            GameManager.Instance.BotDeath(this);
        
        if(move)
            move.PlayDead();

        deathEffect.SetActive(true);
        
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //Destroy(gameObject, 9);
        deathAudioSource.Play();

        ////Invoke(nameof(Death),2.33f);
        Death();
    }

    void Death()
    {
        //MyChange

        ////int i = isGiant ? Random.Range(3,6): Random.Range(1, 4);
        ////for (int ii = 0; ii < i; ii++)
        ////{
        ////    Instantiate(deathBonus, transform.position + Vector3.up, transform.rotation);
        ////}

       /* if (IsChest)
        {
            if (IsPickUp)
            {
                StartCoroutine(SpwanPickUp());
            }
            else
            {
                int i = UnityEngine.Random.Range(2, 4);
                for (int ii = 0; ii < i; ii++)
                {
                    Instantiate(deathBonus, transform.parent.position, Quaternion.identity);
                }
            }



            Invoke(nameof(DisableChest), 0.5f);
        }
        else
        {
            gameObject.SetActive(false);
        }
*/
        gameObject.SetActive(false);

        //MyChange
        if (IsChest)
        {
            Destroy(gameObject);

            int i = UnityEngine.Random.Range(2, 4);
            for (int ii = 0; ii < i; ii++)
            {
                Instantiate(deathBonus, transform.parent.position, Quaternion.identity);
            }
        }
    }

    //IEnumerator SpwanPickUp()
    //{
    //    //yield return new WaitForSeconds(0.2f);
    //    GameObject go = Instantiate(deathBonus, transform.parent.position + new Vector3(0f, 0f, 0f), Quaternion.identity);

    //    yield return null;
        
    //}


    void DisableChest()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void ResetBot()
    {
        if(IsChest)
            return;

        isAlive = true;
        ResetHealth();
        //move.animator.SetBool("Dead", false);
        deathEffect.SetActive(false);

        health = healthActual;
    }

   

}
