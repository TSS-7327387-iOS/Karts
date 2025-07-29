
using UnityEngine;

[CreateAssetMenu(fileName = "GrimReapper", menuName = "ScriptableObjects/Ability/HealthSnatcher")]
public class HealthSnatcherAbility : Ability
{

    public override void StartAttck(Transform target, Character charac)
    {
        charac.power.isActive = false;
        Instantiate(effect, target.position, target.rotation);
        charac.isAnimatingPower = false;
        charac.power.HealthSnatch(radius);
    }

}
