using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;
using Exploder.Utils ;
using Tarodev;

public class PlayerAllRef : MonoBehaviour
{
    public static PlayerAllRef instance;
    public Kart kartRef_Script;
   
    public ParticleSystem coinsParticles;

    // Start is called before the first frame update
    private void Awake()
    {
        instance= this;
       
    }
   
    public void playCoinsParticles()
    {
        coinsParticles.Play();
    }

   

}
