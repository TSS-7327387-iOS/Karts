using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


public class MiniPlayers : MonoBehaviour
{

    public float velocity = 5f;
    public float gravity = 9.8f;

    // Inputs
    float inputHorizontal;
    float inputVertical;
    public PlayerInput playerInput;
    public ThirdPersonController player;
    public Animator animator;
    public CharacterController cc;
    public NavMeshAgent agent;
    public float followDistance;
    public MinionDetector detector;
    Pickups pick;
    Character parent;
    
    //public bool isAnimating;
    public void Initialize(Character chhh)
    {
        cc = GetComponent<CharacterController>();
        parent = chhh;
        detector.chhh = chhh;
        player = GameManager.Instance.thirdPersonController;
    }

    float animationValue;
    // Update is only being used here to identify keys and trigger animations
    void Update()
    {
        /*if (layingDown == true || animator == null)
            return;*/

        // Input checkers
        inputHorizontal = playerInput.actions["Move"].ReadValue<Vector2>().x;//Input.GetAxis("Horizontal");
        inputVertical = playerInput.actions["Move"].ReadValue<Vector2>().y;//Input.GetAxis("Vertical");

        moving = cc.velocity.magnitude > 0.2f;
        animator.SetBool("Move",moving ||(agent.enabled && agent.remainingDistance>agent.stoppingDistance));

        if (Vector3.Distance(transform.position, parent.transform.position) > followDistance)
        {
            cc.enabled = false;
            agent.enabled = true;
            fight = false;
            agent.SetDestination(parent.transform.position + parent.transform.forward);
        }
        else
        {
            if (!fight && !moving)
            {
                cc.enabled = false;
                agent.enabled = true;
                agent.SetDestination(parent.transform.position + parent.transform.forward);
            }
            else
            {
                agent.enabled = fight;
                cc.enabled = moving && !fight;
            }
        }
        animator.SetBool("Fight", fight);

    }

    private void FixedUpdate()
    {
        if (player.layingDown == true)
            return;

        float velocityAdittion = 0;


        // Direction movement
        float directionX = inputHorizontal * (velocity + velocityAdittion) * Time.deltaTime;
        float directionZ = inputVertical * (velocity + velocityAdittion) * Time.deltaTime;
        float directionY = 0;


        // Add gravity to Y axis
        directionY = directionY - gravity * Time.deltaTime;


        // --- Character rotation --- 

        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Relate the front with the Z direction (depth) and right with X (lateral movement)
        forward = forward * directionZ;
        right = right * directionX;

        if (directionX != 0 || directionZ != 0)
        {
            float angle = Mathf.Atan2(forward.x + right.x, forward.z + right.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.15f);
        }

        // --- End rotation ---


        Vector3 verticalDirection = Vector3.up * directionY;
        Vector3 horizontalDirection = forward + right;

        Vector3 moviment = verticalDirection + horizontalDirection;
        if(cc.enabled) cc.Move(moviment);

    }
    bool fight, moving;
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character charac))
        {
            if (charac == parent) return;
            fight = charac.isAlive;
            if (fight && !moving) agent.SetDestination(charac.transform.position);
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
