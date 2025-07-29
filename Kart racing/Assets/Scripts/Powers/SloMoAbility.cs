using UnityEngine;


[CreateAssetMenu(fileName = "StunGun", menuName = "ScriptableObjects/Ability/Stunning")]
public class SloMoAbility : Ability
{
    public override void StartAttck(Transform target, Character charac)
    {
        
        charac.power.isActive = false;
        Instantiate(effect, target.position, target.rotation);
        Vector3 p1 = target.position;
        charac.isAnimatingPower = false;
        charac.power.PlayPowerSound();
        charac.power.InitiateStunn(duration);
        Collider[] hitColliders = Physics.OverlapSphere(p1, radius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent<Character>(out Character ch))
            {
                if (ch != charac && !ch.isBot)
                {
                    ch.power.StunnEffect(duration);
                }
            }

        }
        

    }


}
