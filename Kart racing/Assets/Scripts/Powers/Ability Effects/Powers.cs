using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powers : MonoBehaviour
{
    public Character character;
    public AImovement move;
    public ThirdPersonController moveController;
    public bool isActive;
    [HideInInspector] public bool inVulnerability;
    protected float ocRunning, ocChasing, ocPlayerVelocity;
    public AudioSource audioSourceP;
    public float maxSound = 35;
    public float orgAnimeSpeed;
    protected void Initialize()
    {
        isActive = true;
        inVulnerability = false;
        SetRefernce();
        audioSourceP = gameObject.AddComponent<AudioSource>();

        // Assign the AudioClip to the AudioSource
        if (character.specialPower.powerAudio != null) audioSourceP.clip = character.specialPower.powerAudio;
        audioSourceP.spatialBlend = 1.0f;
        audioSourceP.minDistance = 1.0f;
        audioSourceP.maxDistance = maxSound;
        audioSourceP.loop = true;
        audioSourceP.playOnAwake = false;
        orgAnimeSpeed = character.animator.speed;
    }
    public void PlayPowerSound()
    {
        if (character.specialPower.powerAudio != null)
        {
            audioSourceP.Play();
            Invoke(nameof(ShutAudioOff), character.specialPower.powerAudioLength);
        }
    }

    public void ShutAudioOff()
    {
        audioSourceP.Stop();
    }
    void SetRefernce()
    {
        if (character.isEnemy)
        {
            move = GetComponent<AImovement>();
            ocRunning = move.runningSpeed;
            ocChasing = move.chasingSpeed;
        }
        else
        {
            //moveController = GetComponent<ThirdPersonController>();
            ocPlayerVelocity = moveController.velocity;
        }
    }

    #region Speedster
    public virtual void StartSpeed(float delay)
    {
        /* if (character.isEnemy)
         {
             move.runningSpeed = speed;
             move.chasingSpeed = speed;
         }
         else
         {
             moveController.velocity = speed;
         }
         character.animator.speed *= 2;
         Invoke(nameof(ResetSpeed), delay);*/
    }
    public virtual void ResetSpeed()
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
        character.animator.speed /= 2;
    }
    #endregion

    #region Stunn
    public virtual void InitiateStunn(float delay)
    {

    }
    public  void StunnEffect(float delay)  //StunEffect
    {
        if (inVulnerability)
        {
            return;
        }
        if (character.isEnemy)
        {
            move.runningSpeed = 0;
            move.chasingSpeed = 0;
        }
        else
        {
            moveController.velocity = 0;
        }
        character.animator.speed /= 2;


        Invoke(nameof(ResetStunn), delay);
    }
    public void ResetStunn()
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
        character.animator.speed = orgAnimeSpeed;
    }

    #endregion
    private void OnDisable()
    {
        ResetStunn();
    }
    #region invulnerability

    public virtual void InVulnerable(float duration)
    {
        inVulnerability = true;
        Invoke(nameof(ResetVulnerbility), duration);
    }
    public virtual void ResetVulnerbility()
    {
        inVulnerability = false;
    }
    #endregion

    #region invisibility

    public virtual void InVisible(float duration)
    {

    }
    public virtual void ResetVisibility()
    {

    }
    #endregion

    #region Flight

    public virtual void Flight(float duration)
    {

    }
    public virtual void ResetFlight()
    {

    }
    #endregion

    #region DeployDrones

    public virtual void DeployDrones(float duration)
    {

    }
    public virtual void ResetDeployDrones()
    {

    }
    #endregion

    #region FirePunch

    public virtual void FireAttack(Transform duration)
    {

    }

    #endregion
    #region HighJump

    public virtual void JumpHigh(float radius, float time)
    {

    }

    #endregion
    #region HighJump

    public virtual void HealthSnatch(float radius)
    {

    }

    #endregion

    #region ShapeShifter(Mochojin)

    public virtual void MochoJinAttack(float radius, float time)
    {

    }

    #endregion


}
