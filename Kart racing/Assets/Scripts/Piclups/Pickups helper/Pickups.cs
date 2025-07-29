using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    [HideInInspector] public Character character;
    AImovement move;
    ThirdPersonController moveController;
    protected float ocRunning, ocChasing, ocPlayerVelocity;

    private void Start()
    {
        character = GetComponent<Character>();
        if (character.isEnemy)
        {
            move = GetComponent<AImovement>();
            ocRunning = move.runningSpeed;
            ocChasing = move.chasingSpeed;
        }
        else
        {
            moveController = GetComponentInParent<ThirdPersonController>();
            ocPlayerVelocity = moveController.velocity;
        }
    }

    #region Booster

    float orgSpeed;
    public  void StartSpeed(float delay, float boostSpeed)
    {
        character.isAnimatingPower = false;
        if (character.isEnemy)
        {
            move.runningSpeed = boostSpeed;
            move.chasingSpeed = boostSpeed;
        }
        else
        {
            moveController.velocity = boostSpeed+3;
        }
        orgSpeed = character.animator.speed;
        character.animator.speed *= 2;
        Invoke(nameof(ResetSpeed), delay);
    }
    public void ResetSpeed()
    {
        if (character.isEnemy)
        {
            move.runningSpeed = ocRunning;
            move.chasingSpeed = ocChasing;
        }
        else
        {
            moveController.velocity = ocPlayerVelocity;
        }
        character.animator.speed = orgSpeed;
    }

    #endregion

    #region MinnionsShield
    List<Minions> minionsAlive = new List<Minions>();
    List<MiniPlayers> playerMinionsAlive = new List<MiniPlayers>();
    public void MinionSpwaned(Minions mini,float delay)
    {
        minionsAlive.Add(mini);
        Invoke(nameof(RemoveMinion),delay);
    }
    public void RemoveMinion()
    {
        if (minionsAlive[0] == null) return;
        var mini = minionsAlive[0];
        minionsAlive.Remove(mini);
        Destroy(mini.gameObject);
    }
    public void PlayersMinionSpwaned(MiniPlayers mini,float delay)
    {
        playerMinionsAlive.Add(mini);
        Invoke(nameof(PlayersRemoveMinion),delay);
    }
    public void PlayersRemoveMinion()
    {
        if (playerMinionsAlive[0] == null) return;
        var mini = playerMinionsAlive[0];
        playerMinionsAlive.Remove(mini);
        Destroy(mini.gameObject);
    }
    #endregion

    #region Mines
    int mines;
    public void StockMines()
    {
        mines++;
        if (!character.isEnemy) UIManager.Instance.UpdateMinesStack(mines);
    }
    public void DeployMines()
    {
        if (mines > 0)
        {
            if(!character.isEnemy)
                Instantiate(GameManager.Instance.powerUpsManager.Mine, transform.position+(transform.forward * -1), Quaternion.identity);
            else
                Instantiate(GameManager.Instance.powerUpsManager.Mine, transform.position + (transform.forward * -1)+new Vector3(0,0.33f,0), Quaternion.identity);
            mines--;
            UIManager.Instance.UpdateMinesStack(mines);

            //print("--------------Mine Deploy-------------");
        }
    }

    #endregion

}

[Serializable]
public enum PickupCat
{
    Booster = 2,
    Shield = 3,
    Mine = 1,
    None = 0
}