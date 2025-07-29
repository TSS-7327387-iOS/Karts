using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "LightingFast", menuName = "ScriptableObjects/Ability/Speedster")]
public class LightingAbility : Ability
{
    
    public override void StartAttck(Transform target, Character ch)
    {
        ch.power.isActive = false;
        Instantiate(effect, target.position, target.rotation);
        ch.power.StartSpeed(duration);
        //animator.SetTrigger("Special");

    }

}
