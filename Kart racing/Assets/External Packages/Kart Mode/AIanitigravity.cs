using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

public class AIanitigravity : MonoBehaviour
{
    public bool isAI, useAntiGravityForAI;
    public KartGravityPreset antiGravPreset;
    public static AIanitigravity instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (isAI && useAntiGravityForAI)
        {
            GetComponent<KartPresetControl>().LoadGravityPreset(antiGravPreset);
            GetComponent<Rigidbody>().useGravity = false;
        }
    }


}
