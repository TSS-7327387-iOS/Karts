
using UnityEngine;

[CreateAssetMenu(fileName = "Invisibility", menuName = "ScriptableObjects/Ability/Invisible")]
public class InvisibleAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.isAnimatingPower = false;
        ch.power.isActive = false;
        Instantiate(effect, target.position,target.rotation);
        ch.power.InVisible(duration);
        //animator.SetTrigger("Special");

    }
}
