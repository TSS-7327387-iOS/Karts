
using UnityEngine;

[CreateAssetMenu(fileName = "Fire Punch", menuName = "ScriptableObjects/Ability/FirePunch")]

public class FirePunchAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.power.isActive = false;
        //Instantiate(effect, target.position, target.rotation);
        ch.power.FireAttack(target);

    }
}