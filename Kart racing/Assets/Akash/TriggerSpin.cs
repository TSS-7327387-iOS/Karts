using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

public class TriggerSpin : MonoBehaviour
{
    public bool isCoustom;
    public Kart.SpinAxis spinType;
    public int count;
    private void OnTriggerExit(Collider other)
    {
       /*if( other.TryGetComponent<PlayerAllRef>(out PlayerAllRef playerAllRef)){
            Debug.Log("Trigger Spin Detected player kart");
            playerAllRef.kartRef_Script.call_SpinOutCustom();

        }*/
        if (other.GetComponentInParent<PlayerAllRef>()!=null)
        {
            PlayerAllRef playerAllRef = other.GetComponentInParent<PlayerAllRef>();
            Debug.Log("Trigger Spin Detected player kart");
            if(!isCoustom)playerAllRef.kartRef_Script.call_SpinOutCustom();
            else
            {
                playerAllRef.kartRef_Script.SpinOutCustom(spinType, count);
            }
        }
    }
}
