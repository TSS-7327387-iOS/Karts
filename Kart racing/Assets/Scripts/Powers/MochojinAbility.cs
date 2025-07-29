
using UnityEngine;

[CreateAssetMenu(fileName = "Drones", menuName = "ScriptableObjects/Ability/DeployDrones")]
public class MochojinAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.power.isActive = false;
        if (effect != null) Instantiate(effect, target.position, target.rotation, target);
        ch.power.MochoJinAttack(radius,duration);
        //animator.SetTrigger("Special");
    }
}
