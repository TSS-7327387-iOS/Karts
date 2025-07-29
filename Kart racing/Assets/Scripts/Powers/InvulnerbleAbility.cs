
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Invulnerability", menuName = "ScriptableObjects/Ability/Invulnerable")]
public class InvulnerbleAbility : Ability
{
    public override void StartAttck(Transform target, Character ch)
    {
        ch.isAnimatingPower = false;
        ch.power.isActive = false;
        Instantiate(effect, target.position, target.rotation, target);
        ch.power.InVulnerable(duration);
        //animator.SetTrigger("Special");

    }
}
