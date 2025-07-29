using UnityEngine;

public class MidpointTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        LapCounter lapCounter = other.GetComponentInParent<LapCounter>();
        if (lapCounter != null)
        {
          //  Debug.LogError("Midpoint"+lapCounter.gameObject.name);
            lapCounter.PassMidpoint(other);
        }
    }
}