using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class HighJump : Powers
{
    public NavMeshAgent agent;
    public float jumpHeight;
    [SerializeField]Rigidbody rb;
    private IEnumerator jumpCoroutine;
    Vector3 JumpStartPoint;
    Vector3 JumpMidPoint;
    Vector3 JumpEndPoint;
    List<Vector3> Path = new List<Vector3>();
    float JumpDistance;
    private void Start()
    {
        base.Initialize();
        if (character.isEnemy)
        {
            if (agent == null) agent = GetComponent<NavMeshAgent>();
            if (rb == null) rb = GetComponent<Rigidbody>();
        }
       
    }
    Vector3 endPoint;
    float _radius;
    public override void JumpHigh(float r, float d)
    {
        _radius = r;
        if (character.isEnemy)
        {
            getRandomPoint();
        }
        else
        {
            moveController.jumpTime = d;
            moveController.isJumping = true;
            character.animator.SetBool("Flight", true);
        }
        PlayPowerSound();
    }

  
    void getRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        endPoint = randomDirection;
        if (NavMesh.SamplePosition(randomDirection, out hit, _radius, 1))
        {
            //StartCoroutine(LerpValue());
            endPoint = hit.position;
        }
        //agent.SetDestination( hit.position);
        //jumpCoroutine = jump();
        //StartCoroutine(jumpCoroutine);
        JumpStartPoint = transform.position;

        NavMeshPath hostAgentPath = new NavMeshPath();
        agent.CalculatePath(endPoint, hostAgentPath);
        var endPointIndex = hostAgentPath.corners.Length - 1;
        if(endPointIndex >= 0)
            SpawnAgentAndGetPoint(hostAgentPath.corners[endPointIndex]);
        else
            getRandomPoint();
    }
    
    void SpawnAgentAndGetPoint(Vector3 pos)
    {
        JumpEndPoint = pos;
        character.animator.SetBool("Flight", true);
        //MyChange
        if (character.isEnemy)
        {
            character.animator.SetTrigger("Flight");
        }
        if (!character.isEnemy) agent.enabled = false;
        MakeJumpPath();
    }

    void MakeJumpPath()
    {
        Path.Add(JumpStartPoint);

        var tempMid = Vector3.Lerp(JumpStartPoint, JumpEndPoint, 0.5f);
        tempMid.y = tempMid.y + agent.height + jumpHeight;

        Path.Add(tempMid);

        Path.Add(JumpEndPoint);

        JumpDistance = Vector3.Distance(JumpStartPoint, JumpEndPoint);

        DoJump();
    }

    void DoJump()
    {
        agent.enabled = false;
        Vector3[] _jumpPath;
        _jumpPath = Path.ToArray();

        // if you don't want to use a RigidBody change this to
        //transform.DoLocalPath per the DoTween doc's
        rb.DOLocalPath(_jumpPath, character.specialPower.duration, PathType.CatmullRom).OnComplete(JumpFinished);
    }


    /*   IEnumerator jump()
        {
            //agent.ResetPath();
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
            Vector3 updatedPos;
        // make the jump
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddRelativeForce(new Vector3(0, jumpHeight, 0), ForceMode.Impulse);
            yield return new WaitForSeconds(0.11f);
            isJumping = true;
            float duration = character.specialPower.duration; // Total duration for the lerp (in seconds)
            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                updatedPos = new Vector3(endPoint.x,transform.position.y,endPoint.z);
                transform.position = Vector3.Lerp(transform.position, updatedPos, Time.deltaTime * speed);
                yield return null;
            }
        }*/
    void JumpFinished()
    {
        agent.nextPosition = transform.position;
        agent.enabled = true;
        Path.Clear();
        character.animator.SetBool("Flight", false);
        //MyChange
        if (character.isEnemy)
        {
            character.animator.SetTrigger("Run");
        }
        character.isAnimatingPower = false;
    }
 /*   private void OnTriggerEnter(Collider other)
    {
        if (isJumping)
        {
            if (other.CompareTag("Ground"))
            {
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.isStopped = false;
                // make the jump
                rb.isKinematic = true;
                rb.useGravity = false;
                isJumping = false;
                StopCoroutine(jumpCoroutine);
                character.animator.SetBool("Flight", false);
                agent.nextPosition = transform.position;
            }
        }
    }*/
    /*  IEnumerator LerpValue()
      {
          float duration = character.specialPower.duration/2; // Total duration for the lerp (in seconds)
          float t = 0f;
          agent.baseOffset = 0.5f;
          while (t < 1)
          {
              t += Time.deltaTime / duration;
              agent.baseOffset = Mathf.Lerp(0.5f, 3f, t);
              transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * speed);
              yield return null;
          }
          transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime*speed);
          yield return new WaitForSeconds(0.5f); // Wait for a second before lerping back
          transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * speed);
          t = 0f; // Reset t for the reverse lerp

          while (t < 1)
          {
              t += Time.deltaTime / duration;
              agent.baseOffset = Mathf.Lerp(3f, 0f, t);
              transform.position = Vector3.Lerp(transform.position, endPoint, Time.deltaTime * speed);
              yield return null;
          }
          agent.speed = orgSpeed;
          character.animator.SetBool("Flight", false);
      }*/
}
