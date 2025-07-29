
using UnityEngine;


public class Mine : MonoBehaviour
{
    public float blastRadius;
    public int damage;
    public LayerMask layerMask;
    public GameObject effect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") || other.CompareTag("Player") || other.CompareTag("Bot") )
        {
            effect.SetActive(true);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius, layerMask);
            foreach (Collider col in hitColliders)
            {
                if(col.TryGetComponent<Character>(out Character chh))
                {
                    chh.TakeDamage(damage);
                }
            }
            Destroy(gameObject, 0.45f);
        }
    }
}
