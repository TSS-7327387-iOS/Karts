
using UnityEngine;

public class Speed : Powers
{
    [SerializeField]float boostSpeed;
    float orgSpeed;
    private void Start()
    {
        base.Initialize();
        orgSpeed = character.animator.speed;
    }
    public override void StartSpeed(float delay)
    {
        character.isAnimatingPower = false;
        if (character.isEnemy)
        {
            move.runningSpeed = boostSpeed;
            move.chasingSpeed = boostSpeed;
        }
        else
        {
            moveController.velocity = boostSpeed;
        }
        character.animator.speed *= 2;
        Invoke(nameof(ResetSpeed), delay);
        PlayPowerSound();
    }

    public override void ResetSpeed()
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
    private void OnDisable()
    {
        CancelInvoke();
        ResetSpeed();
        ShutAudioOff();
    }
}
