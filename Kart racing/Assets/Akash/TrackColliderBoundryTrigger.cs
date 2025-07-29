using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

public class TrackColliderTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Kart>() != null)
        {
            Kart kart = other.GetComponentInParent<Kart>();
            Debug.Log("Kart Collided with boundry trigger");
            if (kart.TryGetComponent<LapCounter>(out LapCounter lapCounter))
            {
                kart.call_ResetPositionOnTriggerBoundary(MyGameManager.instance.waypointManager.GetWaypointTransformByID(lapCounter.curntID));
              

            }



        }
    }
}
