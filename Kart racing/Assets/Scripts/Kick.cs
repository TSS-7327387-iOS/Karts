using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{
    public bool IsPlayer;
    EnemyMovement enemyMovement;
    EnemyAI enemyAI;
    bool IsKick;

    private void Start()
    {
        enemyMovement = transform.root.GetComponent<EnemyMovement>();
        enemyAI = GetComponentInParent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer)
        {
            if (other.transform == enemyAI.transform)
                return;

            if (other.CompareTag("Enemy") && enemyMovement.IsPush && !IsKick)
            {
                Debug.Log("---------------Kick Enemy To Enemy --------------");
                IsKick = true;
                Invoke(nameof(ResetKick), 3f);

                if (!GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured)
                {
                    print("Making other character drop ball");
                    other.GetComponent<EnemyAI>().DropBall(transform.position - other.transform.position);
                    print(other.GetComponent<EnemyAI>()._name);
                }
            }

            if (other.CompareTag("Player") && enemyMovement.IsPush && !IsKick && GameManager.Instance.playerHasBalls)
            {
                Debug.Log("---------------Kick Enemy To Player --------------");
                IsKick = true;
                Invoke(nameof(ResetKick), 3f);

                ////if (GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured && enemyAI.move.ChasingPlayer)
                if (GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured)
                {
                    if(enemyAI.move.ChasingPlayer || enemyAI.move.findball)
                    {
                        enemyAI.player.playerGotTouched(transform.position - other.transform.position);
                    }
                }
            }
        }
        

        if (IsPlayer)
        {
            if (other.CompareTag("Enemy") && !GameManager.Instance.playerHasBalls && !IsKick)
            {
                Debug.Log("---------------Kick Player To Enemy --------------");
                IsKick = true;
                Invoke(nameof(ResetKick), 3f);

                if (!GameManager.Instance.Ball.holdedByPlayer && !GameManager.Instance.Ball.ballCaptured)
                    other.GetComponent<EnemyAI>().DropBall(transform.position - other.transform.position);
            }
        }
        

        
    }

    void ResetKick()
    {
        IsKick = false;
    }
}
