using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitReactiveProp : MonoBehaviour
{
    
    
    [Header("Hit Settings")]
    public float hitForce = 10f;
    public float upwardForce = 2f;
    public bool addTorque = true;
    public float physicsDuration = 3f; // How long physics should be active
    
    private bool hasBeenHit = false;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenHit) return;
    
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy"))
        {
            hasBeenHit = true;
    
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
    
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
            Vector3 force = hitDirection * hitForce + Vector3.up * upwardForce;
    
            rb.AddForce(force, ForceMode.Impulse);
    
            if (addTorque)
            {
                rb.AddTorque(Random.insideUnitSphere * hitForce, ForceMode.Impulse);
            }
    
            // Remove Rigidbody after physicsDuration
            Invoke(nameof(RemoveRigidbody), physicsDuration);
    
            // Also deactivate GameObject after 3 seconds
            Invoke(nameof(DeactivateProp), 3f);
        }
    }
    
    private void RemoveRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            Destroy(rb);
        }
    
        hasBeenHit = false;
    }
    
    private void DeactivateProp()
    {
        gameObject.SetActive(false);
    }
    
    
}
