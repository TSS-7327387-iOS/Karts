using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsCalling : MonoBehaviour
{
   private void Start()
   {
      TssAdsManager._Instance.admobInstance.ShowLeftBanner();
      TssAdsManager._Instance.admobInstance.ShowRightBanner();
      TssAdsManager._Instance.admobInstance.ShowRecBanner();
   }

   public void intshow()
   {
      TssAdsManager._Instance.ShowInterstitial("Interstitial shown");
   }
}
