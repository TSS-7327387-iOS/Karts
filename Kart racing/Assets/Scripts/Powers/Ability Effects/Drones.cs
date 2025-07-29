using System.Collections.Generic;
using UnityEngine;

public class Drones : Powers
{
    public Animator droneAnimator;
    public ProjectileMover shoots;
    public Transform droneHolder;
    public Transform shootPoint, shootPoint1;
    Vector3 pos;
    Quaternion rot;
    private void Start()
    {
        base.Initialize();
    }
    private void OnEnable()
    {
        droneAnimator.SetBool("reverse", true);
        droneAnimator.transform.parent = droneHolder;
        pos = droneAnimator.transform.localPosition;
        rot = droneAnimator.transform.localRotation;
    }
    public override void DeployDrones(float duration)
    {
        droneAnimator.transform.parent = transform;
        droneAnimator.SetBool("reverse", false);
        droneAnimator.enabled = true;
        PlayPowerSound();
        Invoke(nameof(ResetDeployDrones), duration);
        InvokeRepeating(nameof(Shoot), 1, 0.75f);
        InvokeRepeating(nameof(Shoot1), 1.33f, 0.75f);

    }
   
    public void Shoot()
    {
        var target = FindClosestEnemy(transform.position, GameManager.Instance.enemyManager.enemiesAlive);
        shootPoint.LookAt(target);
      //  Instantiate(shoots, shootPoint.position, shootPoint.rotation).InitializeMissile(character);
    }
    public void Shoot1()
    {
        var target = FindClosestEnemy(transform.position, GameManager.Instance.enemyManager.enemiesAlive);
        shootPoint1.LookAt(target);
       // Instantiate(shoots, shootPoint1.position, shootPoint1.rotation).InitializeMissile(character);
    }
    public override void ResetDeployDrones()
    {
        CancelInvoke();
        droneAnimator.SetBool("reverse", true);
        character.isAnimatingPower = false;
        droneAnimator.transform.parent = droneHolder;
        Invoke(nameof(SetDrone), 2);
    }
    void SetDrone()
    {
        droneAnimator.transform.localPosition = pos;
        droneAnimator.transform.localRotation = rot;
    }
    private void OnDisable()
    {
        character.isAnimatingPower = false;
        CancelInvoke();
        ShutAudioOff();
        droneAnimator.transform.localPosition = pos;
        droneAnimator.transform.localRotation = rot;
    }
    public Transform FindClosestEnemy(Vector3 playerPosition, List<EnemyAI> enemies)
    {
        EnemyAI closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        if(character.isEnemy)
            closestDistance = Vector3.Distance(playerPosition, GameManager.Instance.player.transform.position);
        bool playerClosest = true;
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
        return playerClosest ? GameManager.Instance.player.transform : closestEnemy.transform;
    }

}

