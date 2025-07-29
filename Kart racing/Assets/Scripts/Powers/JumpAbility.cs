
using UnityEngine;

[CreateAssetMenu(fileName = "High Jump", menuName = "ScriptableObjects/Ability/HighJump")]
public class JumpAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.power.isActive = false;
        Instantiate(effect, target.position, target.rotation);
        ch.power.JumpHigh(radius,duration);

    }
}
