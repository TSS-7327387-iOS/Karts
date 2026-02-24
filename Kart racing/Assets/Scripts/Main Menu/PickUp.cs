using UnityEngine;
using UnityEngine.AI;

public class PickUp : MonoBehaviour
{
    public bool holdedByPlayer;
    public Rigidbody rb;
    public Collider col;
    public bool ballCaptured;
    public Waypoint_Indicator indi;
    public Transform[] waypoints;
    public NavMeshAgent agent;
    GameManager GManager;

    PowerUpsManager powerUpsManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();
        GManager = GameManager.Instance;
        currentWaypoint = waypoints[0];


        powerUpsManager = FindObjectOfType<PowerUpsManager>();
    }
    private void OnEnable()
    {
        if(col==null) col = GetComponent<Collider>();
        col.enabled = false;
        setCollision();
       // BallCapturingTimmer();
    }

    private void Update()
    {
        ////agent.enabled = !GManager.ballTaken;
        ////if (!GManager.ballTaken)
        ////{
        ////    agent.SetDestination(GetWayPoint());
        ////}

        //MyChange
        //if (!GManager.ballTaken)
        //{
        //    rb.constraints = RigidbodyConstraints.None;
        //    rb.constraints = RigidbodyConstraints.FreezeRotation;
           
        //    Invoke(nameof(SetBallRB), 3f);
        //}

        //if(GameManager.Instance.ballTaken && gameObject.activeSelf)
        //{
        //    gameObject.SetActive(false);
        //}

    }


    void SetBallRB()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        
    }

    Transform currentWaypoint;
    Vector3 GetWayPoint()
    {

        if (Vector3.Distance(transform.position, currentWaypoint.position) > agent.stoppingDistance + 2)
        {
            return currentWaypoint.position;
        }

        currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
        // if(wayIndex == waypoints.Length) wayIndex = 0;
        return currentWaypoint.position;
    }
    bool activate;
    private void OnTriggerStay(Collider other)
    {
        if (GManager.ballTaken || !activate)
            return;

        if (other.CompareTag("Player"))
        {
            print("pick 1");
            if (other.TryGetComponent<Player>(out Player pp))
            {
                print("pick 2");
                if (pp.canPlayerPick)
                {
                    print("pick 3");
                    CancelInvoke(nameof(BallCapturingTimmer));
                    holdedByPlayer = true;
                    rb.isKinematic = true;
                    coll(false);
                    pp.Pickup();
                    pp.ActivateCrown(true);
                    GManager.BallIsPicked(other.transform);
                    ballCaptured=true;
                    Invoke(nameof(BallCapturingTimmer), 2.5f);
                }
            }
        }
        if (other.CompareTag("Enemy"))
        {
            if (other.TryGetComponent<EnemyAI>(out EnemyAI ee))
            {
                if (ee.canPickBall)
                {
                    CancelInvoke(nameof(BallCapturingTimmer));
                    coll(false);
                    holdedByPlayer = false;
                    ee.PickUpBall();
                    ee.ActivateCrown(true);
                    GManager.BallIsPicked(other.transform);
                    ballCaptured = true;
                    Invoke(nameof(BallCapturingTimmer),2.5f);
                }
            }
        }

        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //MyChange

        //Unparent and Respawn Trophy if player fall in water
        if (other.CompareTag("Fall"))
        {
            transform.SetParent(null, false);
            Transform spwanPoint = powerUpsManager.randomSpots[Random.Range(0, powerUpsManager.randomSpots.Length - 1)];
            transform.SetPositionAndRotation(spwanPoint.position, spwanPoint.rotation);

            GameManager.Instance.playerDroppedBall(Vector3.forward);

            GameManager.Instance.BallDropped();

            Debug.Log("Tophy Fall");
        }
    }

    public void BallCapturingTimmer()
    {
        ballCaptured = false;
    }
    public void setCollision()
    {
        Invoke(nameof(colll),0.65f);
    }
    public void coll(bool val=true)
    {
        col.isTrigger = val;
        activate = val;
    }
    public void colll()
    {
        col.isTrigger = true;
        col.enabled = true;
        activate = true;
    }
    

}

