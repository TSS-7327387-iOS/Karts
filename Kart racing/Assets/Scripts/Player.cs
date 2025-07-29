using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
public class Player : Character
{
    [SerializeField] Transform pickupHolder;
    // public Text pickupText;
    List<Pickups> currentPickups;
    GameObject pickupObject;

    public delegate void EnemyTouch(Vector3 direction);
    public event EnemyTouch playerGotMolested;
    //public DetectPlayer detectPlayerStanding;
    public float detectionRadius;
    public bool canPlayerPick=true;
    public Transform camPosition;
    public ThirdPersonController controller;

    public GameObject defultTrophy;

    // Start is called before the first frame update

    private void Awake()
    {
        _name = PlayerPrefs.GetString("PlayerName","WALI007");
        InitializeAbilty();
    }
    private void OnEnable()
    {
        //currentPickup = Pickups.None;
        animator.enabled = true;
        playerGotMolested += DropPickup;
        //SetAnimationValue(0);
        canPlayerPick = true;
        controller.layingDown = false;
        attackBot = kickBot =false;

        //MyChange
        controller.GetComponent<CharacterController>().enabled = true;

        //GameManager.Instance.Ball.gameObject.SetActive(true);
        //defultTrophy.SetActive(false);

    }


    public bool holderInRange,at,att;
    private void Update()
    {
        //MyChange
        if (GameManager.Instance.playerHasBalls && health <= 0f)
        {
            GameManager.Instance.playerDroppedBall(transform.position);
        }
        if (!isAlive)
            return;

        if (kickBot && !controller.layingDown)
        {
            holderInRange = true;
        }
        /*else if (pickupHere && Vector3.Distance(transform.position, pickup.transform.position) < 1.5f)
        {
            animator.SetBool("BotAttack",!isAnimatingPower && !controller.layingDown);
        }*/
        else if (canPlayerPick)
        {
            holderInRange = checkForHolder();
            //detectPlayerStanding.SetCollider(holderInRange);
        }
        else
        {
            holderInRange = false;
        }

        //MyChange //Temp change
        animator.SetBool("BotAttack", pickupHere && (Vector3.Distance(transform.position, pickup.transform.position) < 1.5f) || (attackBot && !isAnimatingPower && !controller.layingDown));

        //if(health > 0f)

        animator.SetBool("Snatch", !attackBot && holderInRange && !isAnimatingPower && !controller.layingDown);
        

        detectPlayer.SetCollider(holderInRange);

        //If player move other direction during fight with bot
        if (bot && attackBot)
        {
            if (Vector3.Distance(transform.position, bot.transform.position) > 1.75f)
            {
                animator.SetBool("BotAttack", false);
                //Debug.LogError("Player moved away");
                attackBot = false;
                kickBot = false;
                bot = null;
            }
        }

       

        if (attackBot && Vector3.Distance(transform.position, bot.transform.position) < 0.5f) //&& bot._name != "devil" && Vector3.Distance(transform.position, bot.transform.position) < 0.5f)
        {
            //Rotate Player towards bot
            var q = Quaternion.LookRotation(bot.transform.position - transform.position);
            q.x = 0;
            q.z = 0;
            transform.parent.rotation = Quaternion.RotateTowards(transform.rotation, q, 80 * Time.deltaTime);
        }
        


        

    }
    public void InitiateAttack()
    {
        isAnimatingPower = true;
        if (specialPower.type == AbilityType.Attack || specialPower.type == AbilityType.Spell)
            animator.SetTrigger(specialPower.type.ToString());
        else
            Attack();
    }
    public void Attack() //also calling it from animation clip
    {
        print("Attackingggg");
        if (specialPower.type == AbilityType.Attack)
        {
            Transform target = controller.enemyManager.enemyWithBall?.transform;
            if (target == null)
                target = transform;

            specialPower.StartAttck(target, this);
            /* if (controller.enemyManager.enemyWithBall != null)
             {
                 specialPower.StartAttck(controller.enemyManager.enemyWithBall?.transform, this);
             }
             else
             {
                 specialPower.StartAttck(transform, this);
             }*/
        }
        else
            specialPower.StartAttck(transform, this);


        UIManager.Instance.PowerBtn.fillAmount = 0f;
        UIManager.Instance.EnableDisablePowerButton(false);
    }
    public void SetNormal()
    {

    }
    /* public void Pickup(PickupSO pickup)
     {
         if (currentPickup != Pickups.None)
             Destroy(pickupObject);
         pickupSO = pickup;
         var pickupCatagory = pickupSO.pickupType;

        if(pickupCatagory!=Pickups.None)
             ChangePickUp(pickupSO.model);

         currentPickup = pickupCatagory;

        SetAnimationValue(pickupSO.attaackAnimationBlendVal);


     }*/
    public void playerGotTouched(Vector3 dir)
    {
        dir = new Vector3(dir.x, 0, dir.z);
        if (!power.inVulnerability)
        {
            playerGotMolested?.Invoke(dir);
            TakeDamage(20);
        }
    }
    public void Pickup()
    {
        GameManager.Instance.PlayerPickedBall(pickupHolder);

        //MyChange
        if (health > 0)
        {
            GameManager.Instance.Ball.gameObject.SetActive(false);
            defultTrophy.SetActive(true);
        }
        

    }
    void DropPickup(Vector3 dir)
    {
        //MyChange
        GameManager.Instance.Ball.gameObject.SetActive(true);
        defultTrophy.SetActive(false);



        GameManager.Instance.playerDroppedBall(dir);
        canPlayerPick = false;
    }
    /*public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }*/
    public void FallFinish()
    {
        controller.layingDown = false;
        canPlayerPick = true;
    }
    /*void SetAnimationValue(float val)
    {
        controller.ChangeAttackAnimationValue(val);
    }*/
    bool checkForHolder()
    {
        RaycastHit hit;
        print("Hitting raycast");
        if (Physics.SphereCast(transform.position, detectionRadius, transform.forward, out hit, 1))
        {
            if (hit.transform.TryGetComponent<Character>(out Character chaaa))
            {
                if (chaaa == GameManager.Instance.enemyManager.enemyWithBall)
                {
                    if (Vector3.Distance(transform.position, GameManager.Instance.enemyManager.enemyWithBall.transform.position) < 1f)
                    {
                        print("________Detected_____");
                        return true;
                    }
                    
                }
            }
        }
        return false;
    }
    public bool kickBot,attackBot,pickupHere;
    public BotAI bot;
    public Pickable pickup;
    public void PlayDead()
    {
        controller.layingDown = true;
    }
  /*  private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BotAI>(out BotAI bb))
        {
            kickBot = bb.isAlive;
            bot = bb.isAlive ? bb : null;   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            print("Bot exited"+other.name);
            kickBot=false;
            bot = null;
        }
    }*/

    public override void Die()
    {
        //MyChange
        controller.GetComponent<CharacterController>().enabled = false;
        canPlayerPick = false;
        MMVibrationManager.Haptic(HapticTypes.Failure, false, false, this);
        UIManager.Instance.GiveWarning();
        playerGotMolested -= DropPickup;
        GameManager.Instance.PlayerDied();
    }

   /* void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Vector3 dir = transform.position;
        Gizmos.DrawSphere(dir, detectionRadius);
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BotAI>(out BotAI bb))
        {
            if (bb.isAlive && kickBot)
            {
                attackBot = true;
                //Debug.Log("enter bot");


                if (bb._name != "devil") controller.transform.LookAt(bot.transform);
            }
        }
        
    } private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<BotAI>(out BotAI bb))
        {
            attackBot = bb.isAlive;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bot"))
        {
            attackBot = false;
            //Debug.Log("exit bot");
        }
            
    }

}
