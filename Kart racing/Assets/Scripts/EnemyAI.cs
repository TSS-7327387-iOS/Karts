using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class EnemyAI : Character
{
    [SerializeField]NavMeshAgent agent;
    public Player player;
    public Transform target;
    public Transform pickupHolder;
    public EnemyManager manager;
    public EnemyMovement move;
    public GameObject ball;
    public GameManager gManager;
    Pickups pick;
    float chargeTime;



    void Awake()
    {
        if(agent == null)
            agent = GetComponent<NavMeshAgent>();
        gManager = GameManager.Instance;
        player = gManager.player;
    }
    private void Start()
    {
        move = GetComponent<EnemyMovement>();
        manager = gManager.enemyManager;
        chargeTime = specialPower.rechargeTime;
        InitializeAbilty();
        pick = GetComponent<Pickups>();


    }
    private void OnEnable()
    {
        canPickBall = true;
        isAlive = true;
        player.playerGotMolested += findBall;
        findBall(Vector3.forward);
        move.fallen = false;
        isAnimatingPower = false;

        agent.enabled = true;

        
    }

    public void findBall(Vector3 dir)
    {
       move.findBall();
    }
    
    public void ChangeStateToChase(Transform target)
    {
        if(target==this.transform)
        {
            return;
        }
        this.target = target;
        move.ChangeStateToChase();
    }
    float bombCheckTime=10;
    private void Update()
    {
        if (!power.isActive)
        {
            if (chargeTime > 0)
            {
                chargeTime -= Time.deltaTime;
            }
            else
            {
                chargeTime = specialPower.rechargeTime;
                power.isActive = true;
            }
        }
        if(bombCheckTime>0)
        {
            bombCheckTime -= Time.deltaTime;
        }
        else
        {
            pick.DeployMines();
            bombCheckTime = 10;
        }

        //MyChange
        if(move.ballPicked && gManager.ballTaken && health <= 0f)
        {
            DropBall(transform.position);
            //Debug.Log("!!!!!!!!!!!!Enemy Drop Ball!!!!!!!!!!!!!!!!!" + gManager.Ball.gameObject.activeSelf);
        }
    }
    /*public Vector3 GetRandomPoint()
    {
        
        Vector3 randomDirection = Random.insideUnitSphere * manager.enemyRunningRadius;
        randomDirection += transform.position;

        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;

        if (NavMesh.SamplePosition(randomDirection, out hit, manager.enemyRunningRadius, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }*/
    public bool canPickBall;
    GameObject pickupObject;
    public void PickUpBall()
    {
        gManager.Ball.gameObject.SetActive(false);
        gManager.Ball.transform.parent = pickupHolder;
        gManager.enemyManager.enemyWithBall = this;
        ball.SetActive(true);
        move.changeStateToRun();
        canPickBall = false;
        //detectPlayer.enabled = false;

    }
    public void DropBall(Vector3 dir)
    {
        //print("----------------Enemy Touch drop ball 1 ---------------  "+ move.ballPicked);
        //print("----------------Enemy Touch drop ball 2 ---------------  "+ power.inVulnerability);
        //print("----------------Enemy Touch drop ball 2 ---------------  " + _name);


        if (!move.ballPicked || power.inVulnerability) return;
        ball.SetActive(false);

        if (!move.fallen)
            move.fallen = true;
        canPickBall=false;
        //print("---------------- Drop ball fallen ---------------");
        gManager.Ball.transform.parent = null;
        gManager.Ball.gameObject.SetActive(true);
        dir = new Vector3(dir.x, 0, dir.z);
        gManager.Ball.rb.AddForce(dir*10, ForceMode.Force);
        move.Fallen();
        gManager.BallDropped();
        ActivateCrown(false);
        TakeDamage(20);
        //detectPlayer.enabled = true;
        //Invoke(nameof(CanPick),2);
    }

    public void DeactiveDupliucateBall()
    {
        ball.SetActive(false);
        ActivateCrown(false);
        canPickBall = true;
        print("---------------- DeactiveDupliucateBall 1 ---------------");
    }

    public void Attack()
    {
        Debug.Log("$$$$$$$$$$$ ________Special Power Attack_________ $$$$$$$");

        if (specialPower.type == AbilityType.Spell || specialPower.type == AbilityType.Defence)
        {
            specialPower.StartAttck(transform, this);
            isAnimatingPower = true;
        }
        else if (specialPower.type == AbilityType.Attack && gManager.ballTaken)
        {
            isAnimatingPower = true;
            specialPower.StartAttck(gManager.ballHolder, this);
        }//gManager.playerHasBalls ? gManager.player.transform : manager.enemyWithBall.transform, this);
        
    }

    public void SetNormal()
    {
        if (specialPower.type == AbilityType.Spell)
            move.SetAnimationTrasition("Idle");
        move.FallFinish();
        canPickBall = true;
    }
    public override void Die()
    {
        if (!canPickBall || move.ballPicked || manager.enemyWithBall==this) 
        {
            canPickBall=false;
            ball.SetActive(false);
            gManager.Ball.transform.parent = null;
            gManager.Ball.gameObject.SetActive(true);
            gManager.Ball.rb.AddForce(Vector3.forward, ForceMode.Force);
            gManager.ballHolder = null;
            gManager.ballTaken = false;
            gManager.BallDropped();
            ActivateCrown(false);
        }

        //move.ChangeStateToWander();
        //gManager.enemyManager.DeActivateEnemy(this);
        //player.playerGotMolested -= findBall;
        ////gameObject.SetActive(false);
        ////gManager.enemyManager.ResetEnemyPositionRotation(transform);


        


        if (!move.fallen)
        {
            //If Enemy not fall

            move.ChangeStateToWander();
            player.playerGotMolested -= findBall;

            animator.SetLayerWeight(1, 0);
            animator.SetLayerWeight(2, 0);
            animator.SetTrigger("Death");
            agent.enabled = false;

            Invoke(nameof(LateDie), 4f);
        }
        else
        {
            move.ChangeStateToWander();
            player.playerGotMolested -= findBall;

            agent.enabled = false;
            Invoke(nameof(LateDie), 4f);
        }
    }

    void LateDie()
    {
        gameObject.SetActive(false);

        //move.ChangeStateToWander();
        gManager.enemyManager.DeActivateEnemy(this);
        gManager.enemyManager.ResetEnemyPositionRotation(transform);
    }


    public void HoldSword(bool IsHold)
    {
        if (animator != null)
        {
            if (IsHold)
            {
                animator.SetLayerWeight(3, 1);
            }
            else
            {
                animator.SetLayerWeight(3, 0);
            }
            
        }
    }
}
