using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;


public class FlightandGuns : Powers
{
    public NavMeshAgent agent;
    public ProjectileMover shoots;
    public Transform shootPoint;
    public GameObject jetpackEmission;

    public Animator animWing;

    float actualAvoidRadius;

    private void Start()
    {
        base.Initialize();
        if(character.isEnemy && agent == null) agent = GetComponent<NavMeshAgent>();

       
        if(agent != null)
        actualAvoidRadius = agent.radius;
        startYPos = transform.position.y;
    }
    float startYPos;
    public override void Flight(float duration)
    {
        InvokeRepeating(nameof(Shoot), 1, 0.75f);


        character.animator.SetBool("Flight", true);

        

        ////jetpackEmission.SetActive(true);
        animWing.SetTrigger("open");
        if (character.isEnemy)
        {
            character.animator.SetTrigger("Flight");
            agent.radius = 0.02f;
            agent.agentTypeID = GetAgenTypeIDByName("Flight");
            agent.baseOffset = 2; 
            Invoke(nameof(ResetFlight), duration);
        }
        else
        {
            moveController.gameObject.layer = LayerMask.NameToLayer("TransparentFX");
            StartCoroutine(LerpHeight(6, duration));
        }
        PlayPowerSound();
    }

    
    private IEnumerator LerpHeight( float endHeight, float duration)
    {
        float elapsedTime = 0f;
        float startHeight = moveController.cc.height;
        float flightDuration= duration / 4;
        // Lerp to target height
        while (elapsedTime < flightDuration)
        {
            moveController.cc.height = Mathf.Lerp(startHeight, endHeight, elapsedTime / flightDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure we set the exact target height
        moveController.cc.height = endHeight;

        // Wait before coming back to original height
        yield return new WaitForSeconds(duration/2);

        elapsedTime = 0f;

        // Lerp back to original height
        while (elapsedTime < flightDuration)
        {
            moveController.cc.height = Mathf.Lerp(endHeight, startHeight, elapsedTime / flightDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        moveController.gameObject.layer = LayerMask.NameToLayer("Player");
        character.isAnimatingPower = false;
        ////jetpackEmission.SetActive(false);
        animWing.SetTrigger("close");
        character.animator.SetBool("Flight", false);
       
        // Ensure we set back to original height
        CancelInvoke();
        moveController.cc.height = startHeight;
    }

    public void Shoot()
    {
        var target = FindClosestEnemy(transform.position, GameManager.Instance.enemyManager.enemiesAlive);
        shootPoint.LookAt(target);
      //  Instantiate(shoots, shootPoint.position, shootPoint.rotation).InitializeMissile(character);
    }
    public override void ResetFlight()
    {
        CancelInvoke();
        agent.agentTypeID = GetAgenTypeIDByName("Humanoid");
        agent.baseOffset = 0;
        character.isAnimatingPower = false;
        character.animator.SetBool("Flight", false);
        ////jetpackEmission.SetActive(false);
        animWing.SetTrigger("close");

        //MyChange

        character.animator.SetTrigger("Run");
        agent.radius = actualAvoidRadius;
        transform.position = new Vector3(transform.position.x,startYPos,transform.position.z);
    }
    private void OnDisable()
    {
       if(character.isEnemy) ResetFlight();
        else character.isAnimatingPower = false;
       ShutAudioOff();
    }
    public int GetAgenTypeIDByName(string agentTypeName)
    {
        int count = NavMesh.GetSettingsCount();
        string[] agentTypeNames = new string[count + 2];
        for (var i = 0; i < count; i++)
        {
            int id = NavMesh.GetSettingsByIndex(i).agentTypeID;
            string name = NavMesh.GetSettingsNameFromID(id);
            if (name == agentTypeName)
            {
                return id;
            }
        }
        return -1;
    }
    public Transform FindClosestEnemy(Vector3 playerPosition, List<EnemyAI> enemies)
    {
        EnemyAI closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        if (GameManager.Instance.thirdPersonController.enabled && character.isEnemy)
            closestDistance = Vector3.Distance(playerPosition, GameManager.Instance.player.transform.position);
        bool playerClosest=true;
        foreach (var enemy in enemies)
        {
            if (enemy.transform == this.transform)
                continue;
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);

            if (distance < closestDistance)
            {
                closestEnemy = enemy;
                closestDistance = distance;
                playerClosest = false;
            }
        }
        return playerClosest? GameManager.Instance.player.transform: closestEnemy.transform;
    }
}
