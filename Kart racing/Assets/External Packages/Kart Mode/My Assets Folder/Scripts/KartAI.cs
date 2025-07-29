using System.Collections;
using System.Collections.Generic;
using PowerslideKartPhysics;
using UnityEngine;

public class KartAI : MonoBehaviour
{
    ItemCaster itemCaster;
    private bool IsUseAllAmmo = false;

    // Start is called before the first frame update
    void Start()
    {
        itemCaster = GetComponent<ItemCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(itemCaster.item != null)
        {
            if(itemCaster.ammo > 1 && !IsUseAllAmmo)
            {
                StartCoroutine(UseAllAmmo());
                IsUseAllAmmo = true;
            }
            else if(itemCaster.ammo == 1 && !IsUseAllAmmo)
            {
                //itemCaster.Cast();
                //itemCaster.item = null;

                StartCoroutine(UseAmmo());
                IsUseAllAmmo = true;
            }
            
        }
    }

    private IEnumerator UseAllAmmo()
    {
        itemCaster.Cast();
        yield return new WaitForSeconds(0.1f);

        if(itemCaster.ammo != 0)
        {
            StartCoroutine(UseAllAmmo());
        }
        else
        {
            IsUseAllAmmo = false;
            itemCaster.item = null;
        }
    }

    private IEnumerator UseAmmo()
    {
        yield return new WaitForSeconds(2f);
        itemCaster.Cast();
        IsUseAllAmmo = false;
        itemCaster.item = null;
    }
}
