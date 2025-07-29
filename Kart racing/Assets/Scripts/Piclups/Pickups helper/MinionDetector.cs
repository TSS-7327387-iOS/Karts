using UnityEngine;

public class MinionDetector : MonoBehaviour
{
    public Character chhh;
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Character>(out Character cc))
        {
            if ( cc == chhh)
                return;
            cc.TakeDamage(damage);

        }
    }
}
