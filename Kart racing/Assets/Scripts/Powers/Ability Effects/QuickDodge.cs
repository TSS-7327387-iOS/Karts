using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickDodge : Powers
{
    public GameObject damageThing;
    Collider col;
    private void Start()
    {
        base.Initialize();
        col = damageThing.GetComponent<Collider>();
    }
    public override void InVulnerable(float duration)
    {
        inVulnerability = true;
        character.isAnimatingPower = true;
        //moveController.isAnimating = true;
        if (!character.isEnemy)
            character.animator.SetBool("Spell", true);
        else
            character.animator.SetTrigger("Spell");
        Invoke(nameof(ResetVulnerbility), duration);
        PlayPowerSound();
        damageThing.SetActive(true);
        SetCollider(true);
    }

    public void SetCollider(bool val) //Player Function
    {
        col.enabled = val;
        col.isTrigger = val;
    }

    public override void ResetVulnerbility()
    {
        if(!character.isEnemy)
            character.animator.SetBool("Spell",false);
        else
            character.animator.SetTrigger("Run");
        character.isAnimatingPower = false;
        //moveController.isAnimating = false;
        inVulnerability = false;
        damageThing.SetActive(false);
        SetCollider(false);
    }
    private void OnDisable()
    {
        CancelInvoke();
        ResetVulnerbility();
        ShutAudioOff();
        damageThing.SetActive(false);
        SetCollider(false);
    }
}
