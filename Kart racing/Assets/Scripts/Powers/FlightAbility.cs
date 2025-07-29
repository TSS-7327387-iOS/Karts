using UnityEngine;

[CreateAssetMenu(fileName = "FlightNGuns", menuName = "ScriptableObjects/Ability/Flight")]
public class FlightAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.power.isActive = false;
        if (effect != null) Instantiate(effect, target.position, target.rotation);
        ch.power.Flight(duration);
        //animator.SetTrigger("Special");

    }
}

