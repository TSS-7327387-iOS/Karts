using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
      

        if (other.GetComponentInParent<PlayerAllRef>() != null)
        {
            PlayerAllRef playerAllRef = other.GetComponentInParent<PlayerAllRef>();
            playerAllRef.playCoinsParticles();
            CurrencyManager.instance.AddLevelCoin(10);
            if(AudioManagerNew.instance != null)
            {
                AudioManagerNew.instance.PlaySound("CoinPick");
            }
            Destroy(gameObject);
        }
    }
}
