using UnityEngine;

public class ReverseTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.LogError($"reverse triggered by: {other.name}");
        LapCounter lapCounter = other.GetComponentInParent<LapCounter>();
        if (lapCounter != null)
        {
            lapCounter.ResetLapProgress();
        }
    }
}