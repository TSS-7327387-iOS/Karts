using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : AImovement
{
    #region DataField

    [Space(10f)]
    [Header("References")]
    [Space(5f)]

    [SerializeField] Transform[] waypoints;
    public bool botHere,pickUpHere;
    public BotAI botChasing;
    public Pickable pickTarget;
    public bool ChasingPlayer, ballPicked, findball, wander;
    

    int wayIndex;
    private float actualAgentAvoidRadius;

    [Space(10f)]
    [Header("Debug")]
    [Space(5f)]

    [SerializeField] Transform currentWaypoint;

    [SerializeField] bool startBotFight;
    public bool IsPush;
    public List<Transform> nearbyAgents = new List<Transform>();

    #endregion



    #region Methods

    private void OnEnable()
    {
        startBotFight = false;
        botHere = false;
        SetAnimationTrasition("Run");
    }

    void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if(enemy == null)
            enemy = GetComponent<EnemyAI>();

        if(animator == null)
            animator = GetComponent<Animator>();

        Invoke(nameof(Init), 0.2f);

        actualAgentAvoidRadius = agent.radius;
    }


    private void Update()
    {
        if (fallen || !agent.enabled|| !enemy.isAlive)
            return;

        if (agent.velocity.magnitude < 0.1f || enemy.isAnimatingPower)
            animator.SetLayerWeight(2, 0);
        else
            animator.SetLayerWeight(2, 1);


        if (botHere && !ballPicked)
        {
            FightWithBot();
            if(botChasing != null)
                enemy.detectPlayer.detectTarget = botChasing.transform;
        }
        else if (pickUpHere && !ballPicked)
        {
            AttackChest();
            if (pickTarget != null)
                enemy.detectPlayer.detectTarget = pickTarget.transform;
        }
        else if (!botHere && startBotFight)
        {
            agent.speed = runningSpeed;
            SetAnimationLayer(1, 0f);
            SetAnimationTrasition("Run");

            startBotFight = false;
        }
        else if (ChasingPlayer && enemy.target != null)
        {
            animator.SetLayerWeight(2, 0);
            ChasePlayerTarget();
        }
        else if (ballPicked)
        {
            BallPicked();
        }
        else if (findball)
        {
            FindBall();
        }
        else
        {
            FollowWayPoints();
        }

        //if(agent.hasPath && agent.velocity.magnitude<0.15f) CheckAgents();
        //Check for duplicate trophy
        if (!ballPicked && enemy.ball.activeSelf)
        {
            print("---------------- DeactiveDupliucateBall ---------------");

            //////enemy.DropBall(Vector3.forward);
            enemy.DeactiveDupliucateBall();
        }
    }

    void Init()
    {
        waypoints = enemy.manager.enemyWaypoints;
        GetWayPoint();

        EnemyAgentTargetDestination(currentWaypoint);
        
    }

    Vector3 GetWayPoint()
    {
        //Get random waypoint
        currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
        return currentWaypoint.position;
    }

    void FollowWayPoints()
    {
        //If Enemy reach at current waypoint than get next waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < agent.stoppingDistance + 1)
        {
            GetWayPoint();
            EnemyAgentTargetDestination(currentWaypoint);
        }

        agent.SetDestination(currentWaypoint.position);

        if (IsAnimationPlaying("Idle"))
        {
            SetAnimationTrasition("Run");
        }
    }

    internal void SetAnimationTrasition(string param)
    {
        animator.SetTrigger(param);
    }

    void SetAnimationLayer(int layerIndex, float weight)
    {
        animator.SetLayerWeight(layerIndex, weight);
    }

    void EnemyAgentTargetDestination(Transform target)
    {
        agent.SetDestination(target.position);
        SetAnimationTrasition("Run");
    }
    bool checkPickUp;
    void AttackChest()
    {
        //If bot in fight range then start fight
        if (Vector3.Distance(transform.position, pickTarget.transform.position) < 1.5f)
        {
            agent.ResetPath();

            //If enemy character not performing special power attack
            if (!enemy.isAnimatingPower && !checkPickUp)
            {
                //agent.speed = 0f;
                SetAnimationLayer(1, 1f);
                SetAnimationTrasition("Idle");
                SetAnimationTrasition("Fight");
                checkPickUp = true;
            }

        }
        else
        {
            agent.SetDestination(pickTarget.transform.position);
            checkPickUp=false;
        }

        var q = Quaternion.LookRotation(pickTarget.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 80f * Time.deltaTime);
    }
    void FightWithBot()
    {
        //If bot in fight range then start fight
        if (Vector3.Distance(transform.position, botChasing.transform.position) < botChasing.stoppingDistanceForOthers)
        {
            agent.ResetPath();

            //If enemy character not performing special power attack
            if (!enemy.isAnimatingPower && !startBotFight)
            {
                //agent.speed = 0f;
                SetAnimationLayer(1, 1f);
                SetAnimationTrasition("Idle");
                SetAnimationTrasition("Fight");

                startBotFight = true;
            }

        }
        

        if (botHere)
        {
            //Rotate enemy towards bot
            var q = Quaternion.LookRotation(botChasing.transform.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 80f * Time.deltaTime);
        }

        //If bot not in fight range but in detection range than move
        if (botHere && startBotFight && Vector3.Distance(transform.position, botChasing.transform.position) >= botChasing.stoppingDistanceForOthers)
        {
            agent.speed = runningSpeed;
            SetAnimationLayer(1, 0f);
            SetAnimationTrasition("Run");

            agent.SetDestination(currentWaypoint.position);

            startBotFight = false;
            
        }

        //If bot not in fight range but in detection range than move towards that bot
        //if (botHere && startBotFight && Vector3.Distance(transform.position, botChasing.transform.position) > 1 && botChasing.GetComponent<NavMeshAgent>().velocity.magnitude < 0.1f)
        //{
        //    agent.speed = runningSpeed;
        //    SetAnimationLayer(1, 0f);
        //    SetAnimationTrasition("Run");

        //    agent.SetDestination(botChasing.transform.position);
        //    startBotFight = false;
        //}
    }

    bool IsAnimationPlaying(string stateName)
    {
        if (animator == null) return false;

        // Get the current state info of the animator in layer 0
        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // Check if the current state's name matches the provided name
        return currentStateInfo.IsName(stateName);
    }

    private void ChasePlayerTarget()
    {
        if (enemy.power.isActive && Vector3.Distance(transform.position, enemy.target.position) < stoppingDistance + powerRange)
        {

            if ((enemy.specialPower.type == AbilityType.Attack || enemy.specialPower.type == AbilityType.Spell))
            {
               
                if (Random.Range(0, 9) > 5 && enemy.gManager.ballTaken)
                {
                  
                    transform.LookAt(enemy.target);
                    agent.ResetPath();
                    

                    SetAnimationLayer(1, 0f);
                    if (IsAnimationPlaying("Idle"))
                    {
                        SetAnimationTrasition("Run");
                    }

                    var q = Quaternion.LookRotation(enemy.target.position - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 200f * Time.deltaTime);

                    SetAnimationTrasition(enemy.specialPower.type.ToString());
                    
                    fallen = true;
                    Invoke(nameof(FallFinish), 3);
                    return;
                }
            }
        }
        if (Vector3.Distance(transform.position, enemy.target.position) > stoppingDistance)
        {
           

            SetAnimationLayer(1, 0f);
            if (IsAnimationPlaying("Idle"))
            {
                SetAnimationTrasition("Run");
            }

            agent.speed = chasingSpeed;
            agent.SetDestination(enemy.target.position);

        }
        else if(!IsPush)
        {
            transform.LookAt(enemy.target);
           
            agent.ResetPath();
            

            SetAnimationTrasition("Push");

            IsPush = true;

            Invoke(nameof(IsPushDisable), 3f);
        }

        
    }

    private void BallPicked()
    {
        if (enemy.specialPower.type == AbilityType.Defence)
        {
            if (enemy.power.isActive)
            {
                enemy.Attack();
                currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
                return;
            }
        }

        print("__________Flight Ball Picked _______________");
        if (enemy.isAnimatingPower && enemy.GetComponent<FlightandGuns>() != null)
        {
            if (IsAnimationPlaying("Run"))
            {
                SetAnimationTrasition("Flight");
            }
            agent.speed = runningSpeed;
            if (Vector3.Distance(transform.position, currentWaypoint.position) < agent.stoppingDistance+2)
            {
                GetWayPoint();
            }

            agent.SetDestination(currentWaypoint.position);

            return;
        }
        else
        {
            agent.speed = runningSpeed;
            FollowWayPoints();
        }

       

        ////agent.SetDestination(GetWayPoint());
        ////animator.SetBool("Idle", false);

        ////if (agent.velocity.magnitude < 0.001)
        ////{
        ////    StartCoroutine(PassThroughEnemiemsWhenStuck());
        ////}
        ////else
        ////{
        ////    StopAllCoroutines();
        ////    agent.radius = actualAgentAvoidRadius;
        ////}
    }

    void FindBall()
    {
        if(GameManager.Instance.ballTaken)
            agent.SetDestination(GameManager.Instance.ballHolder.position);
        else
            agent.SetDestination(GameManager.Instance.Ball.transform.position);
        if (IsAnimationPlaying("Idle"))
        {
            SetAnimationTrasition("Run");
        }


        if(enemy.target != null)
        {
            if (Vector3.Distance(transform.position, enemy.target.position) < stoppingDistance)
            {
                if (!IsPush)
                {
                    transform.LookAt(enemy.target);
                    
                    agent.ResetPath();
                    

                    SetAnimationTrasition("Push");

                    IsPush = true;

                    Invoke(nameof(IsPushDisable), 3f);
                }

            }
        }
        

    }

    private void IsPushDisable()
    {
        IsPush = false;
    }

    private IEnumerator PassThroughEnemiemsWhenStuck()
    {
        yield return new WaitForSeconds(2f);
        agent.radius = 0.0001f;
        yield return new WaitForSeconds(5f);
        agent.radius = actualAgentAvoidRadius;
    }

    #endregion

    #region Old Script

    //////public Transform[] waypoints;

    //// public float findingSpeed;

    ////int wayIndex;
    /////Transform currentWaypoint;

    public float changeDirLimit=5;
    ////// public List<Transform> nearbyAgents = new List<Transform>();

    //////public bool botHere;
    //////public BotAI botChasing;

    //////private float actualAgentAvoidRadius;
    // Start is called before the first frame update
    ////void Start()
    ////{
    ////    if (agent == null)
    ////        agent = GetComponent<NavMeshAgent>();
    ////    enemy = GetComponent<EnemyAI>();
    ////    animator = GetComponent<Animator>();

    ////    actualAgentAvoidRadius = agent.radius;
    ////    Invoke("Init", 0.2f);

    ////    chasingSpeed = runningSpeed = findingSpeed = 3.5f;

    ////    ////    waypoints = enemy.manager.enemyWaypoints;
    ////    ////    wayIndex = 1;
    ////    ////    currentWaypoint = waypoints[wayIndex];
    ////}



    ////public bool ChasingPlayer, ballPicked, findball, wander;
    // Update is called once per frame
    //////void Update()
    //////{
    //////    if (fallen || !agent.enabled)
    //////        return;
    //////    if (botHere && !ballPicked)
    //////    {
    //////        animator.SetFloat("AttackBlend", agent.velocity.magnitude);
    //////        if (Vector3.Distance(transform.position, botChasing.transform.position) < botChasing.stoppingDistanceForOthers)
    //////        {
    //////            agent.ResetPath();

    //////            if (botChasing._name != "devil")
    //////                transform.LookAt(botChasing.transform);
    //////            else
    //////                transform.rotation = Quaternion.LookRotation(botChasing.transform.position, transform.up);

    //////            animator.SetBool("Fight", !enemy.isAnimatingPower);
    //////            return;
    //////        }
    //////        agent.speed = runningSpeed;
    //////        animator.SetBool("Idle", false);
    //////        agent.SetDestination(botChasing.transform.position);
    //////        return;
    //////    }
    //////    if (ChasingPlayer && enemy.target != null)
    //////    {
    //////        if (enemy.power.isActive && Vector3.Distance(transform.position, enemy.target.position) < stoppingDistance + powerRange)
    //////        {
    //////            if ((enemy.specialPower.type == AbilityType.Attack || enemy.specialPower.type == AbilityType.Spell))
    //////            {
    //////                if (Random.Range(0, 9) > 5 && enemy.gManager.ballTaken)
    //////                {
    //////                    transform.LookAt(enemy.target);
    //////                    agent.ResetPath();
    //////                    animator.SetBool("Push", false);
    //////                    animator.SetBool("Idle", false);
    //////                    animator.SetTrigger(enemy.specialPower.type.ToString());
    //////                    fallen = true;
    //////                    Invoke(nameof(FallFinish), 3);
    //////                    return;
    //////                }
    //////            }
    //////        }
    //////        if (Vector3.Distance(transform.position, enemy.target.position) > stoppingDistance)
    //////        {
    //////            animator.SetBool("Push", false);
    //////            agent.speed = chasingSpeed;
    //////            // animator.SetBool("Idle", agent.velocity.magnitude < 0.1f);
    //////            agent.SetDestination(enemy.target.position);

    //////        }
    //////        else /*if (!GameManager.Instance.Ball.ballCaptured)*/
    //////        {
    //////            transform.LookAt(enemy.target);
    //////            animator.SetBool("Idle", false);
    //////            agent.ResetPath();
    //////            animator.SetBool("Push", true);
    //////        }
    //////    }
    //////    else if (ballPicked)
    //////    {
    //////        if (enemy.specialPower.type == AbilityType.Defence)
    //////        {
    //////            if (enemy.power.isActive)
    //////            {
    //////                enemy.Attack();
    //////                currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
    //////                return;
    //////            }
    //////        }
    //////        agent.speed = runningSpeed;
    //////        agent.SetDestination(GetWayPoint());
    //////        animator.SetBool("Idle", false);

    //////        if (agent.velocity.magnitude < 0.001)
    //////        {
    //////            StartCoroutine(PassThroughEnemiemsWhenStuck());
    //////            //print("Speed of picked enemy : - :  " + agent.velocity.magnitude);
    //////        }
    //////        else
    //////        {
    //////            StopAllCoroutines();
    //////            agent.radius = actualAgentAvoidRadius;
    //////        }

    //////    }
    //////    else if (findball) //findball
    //////    {
    //////        agent.SetDestination(GameManager.Instance.Ball.transform.position);
    //////        animator.SetBool("Idle", agent.velocity.magnitude < 0.1f);

    //////    }
    //////    else
    //////    {
    //////        agent.SetDestination(GetWayPoint());
    //////    }

    //////    //if(agent.pathStatus== NavMeshPathStatus.PathComplete) CheckAgents();
    //////    animator.SetBool("Fight", false);


    //////    //Check for duplicate trophy
    //////    if (!ballPicked && enemy.ball.activeSelf)
    //////    {
    //////        print("---------------- DeactiveDupliucateBall 2 ---------------");

    //////        enemy.DropBall(Vector3.forward);
    //////        enemy.DeactiveDupliucateBall();
    //////    }
    //////}
    public bool fallen;
    public void Fallen()
    {
        //animator.SetTrigger("fall");
        //animator.SetBool("Fight", false);

        SetAnimationTrasition("Fall");

        if (agent.enabled) agent.ResetPath();
        fallen = true;
        Invoke(nameof(FallFinish), 3);
    }
    public void FallFinish()
    {
        fallen = false;
        enemy.canPickBall = true;
    }
    public void findBall()
    {
        ChasingPlayer = false;
        ////animator.SetBool("Push", false);
        findball = true;
        //agent.speed = findingSpeed;
        ballPicked = false;
    }
    public override void changeStateToRun()
    {
        ////animator.SetBool("Push", false);
        ChasingPlayer = false;
        findball = false;
        ballPicked = true;
        agent.speed = runningSpeed;

    }
    public override void ChangeStateToChase()
    {
        ChasingPlayer = true;
        findball = false;
        ballPicked = false;
        agent.speed = chasingSpeed;
        ////animator.SetBool("Push", false);
    }
    public void ChangeStateToWander()
    {
        ChasingPlayer = false;
        findball = false;
        ballPicked = false;

        ////animator.SetBool("Idle", false);
        ////animator.SetBool("Push", false);
        agent.speed = runningSpeed;
    }

    ////Vector3 GetWayPoint()
    ////{
    ////    if (!gameObject.activeSelf)
    ////        return Vector3.zero;

    ////    if (Vector3.Distance(transform.position, currentWaypoint.position) > agent.stoppingDistance + 2)
    ////    {
    ////        return currentWaypoint.position;
    ////    }

    ////    currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
    ////    // if(wayIndex == waypoints.Length) wayIndex = 0;
    ////    return currentWaypoint.position;
    ////}


    public void CheckAgents()
    {
        if (nearbyAgents.Count == 0)
        {
            return;
        }

        Vector3 directionToClosestLlama = (nearbyAgents[0].position - transform.position);

        agent.velocity = Vector3.Lerp(
            agent.desiredVelocity,
            -directionToClosestLlama.normalized * 5,
            Mathf.Clamp01((6 - directionToClosestLlama.magnitude) / 4)
        );
    }

    //////private IEnumerator PassThroughEnemiemsWhenStuck()
    //////{
    //////    yield return new WaitForSeconds(2f);
    //////    agent.radius = 0.0001f;
    //////    yield return new WaitForSeconds(5f);
    //////    agent.radius = actualAgentAvoidRadius;
    //////}

   /* private void OnTriggerEnter(Collider other)
    {
        if (other.transform == this.transform)
            return;

        if (other.TryGetComponent<EnemyMovement>(out EnemyMovement em))
        {
            if (em.ballPicked)
                return;
        }
        else if (other.CompareTag("Player") && GameManager.Instance.Ball.holdedByPlayer)
        {
            return;
        }

        if (!nearbyAgents.Contains(other.transform))
            nearbyAgents.Add(other.transform);
        if (nearbyAgents.Count > 1)
        {
            nearbyAgents.Sort((a, b) =>
                Vector3.SqrMagnitude(b.position - transform.position)
                    .CompareTo(
                        Vector3.SqrMagnitude(a.position - transform.position)
                    )
            );
        }
    }
      //private void OnTriggerStay(Collider other)
      //{
      //    if (other.transform == this.transform)
      //        return;
      //    if (other.CompareTag("Bot"))
      //    {
      //        if (botChasing != null && !botChasing.isAlive)
      //        {
      //            botHere = false;
      //            return;
      //        }
      //        botHere = true;
      //        botChasing = other.GetComponent<BotAI>();
      //    }
      //}
      private void OnTriggerExit(Collider other)
      {
          if (other.transform == this.transform)
              return;
          if (other.CompareTag("Player") || other.CompareTag("Enemy"))
              if (nearbyAgents.Contains(other.transform))
                  nearbyAgents.Remove(other.transform);

      }
  */
    #endregion

}
