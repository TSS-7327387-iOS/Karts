using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public LapCounter lapCounter;
    private void OnTriggerEnter(Collider other)
    {
        lapCounter = other.GetComponentInParent<LapCounter>();
        if (lapCounter != null)
        {
  //          Debug.LogError($"FinishLine triggered by: {lapCounter.gameObject.name}");
            lapCounter.CrossFinishLine(other);
        }
    }
}