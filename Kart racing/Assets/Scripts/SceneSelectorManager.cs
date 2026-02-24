using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSelectorManager : MonoBehaviour
{
   public GameObject Mainmenu;
   
   [Header("Skyboxes (Materials)")]
   public Material[] skyboxMaterials;

   private void OnEnable()
   {
      TssAdsManager._Instance.ShowBanner("MainMenu Banner");
   }

   private void Start()
   {
      TssAdsManager._Instance.ShowBanner("MainMenu Banner");
   }

   public void ActivateSkybox(int index)
   {
      if (index >= 0 && index < skyboxMaterials.Length)
      {
         RenderSettings.skybox = skyboxMaterials[index];
         DynamicGI.UpdateEnvironment(); // Good practice if your lighting depends on skybox
      }
      else
      {
         Debug.LogError("Skybox index out of range!");
      }
   }

   public void  Mainmenuclose()
   {
      StartCoroutine(ShutMainmenu());
   }

   public IEnumerator ShutMainmenu()
   {
      yield return new WaitForSeconds(5f);
      Mainmenu.SetActive(false);
   }
}
