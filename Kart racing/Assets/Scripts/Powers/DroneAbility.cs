using UnityEngine;

[CreateAssetMenu(fileName = "Drones", menuName = "ScriptableObjects/Ability/DeployDrones")]
public class DroneAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.isAnimatingPower = false;
        ch.power.isActive = false;
        if (effect != null) Instantiate(effect, target.position, target.rotation,target);
        ch.power.DeployDrones(duration);
        //animator.SetTrigger("Special");
    }
}