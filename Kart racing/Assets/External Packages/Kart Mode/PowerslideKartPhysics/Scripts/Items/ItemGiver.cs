// Copyright (c) 2023 Justin Couch / JustInvoke
using System.Collections;
using UnityEngine;

namespace PowerslideKartPhysics
{
    [DisallowMultipleComponent]
    // Class for objects that give items to karts when touched
    public class ItemGiver : MonoBehaviour
    {
        ItemManager manager;
        Collider trig;
        Renderer rend;
        public string itemName;
        public int ammo = 1;
        public float cooldown = 1.0f;
        float offTime = 0.0f;


        private GameObject vfx;
        

        private void Awake() {
            manager = FindObjectOfType<ItemManager>();
            trig = GetComponent<Collider>();
            rend = GetComponent<Renderer>();
            offTime = cooldown;

            if(transform.childCount > 0)
            vfx = transform.GetChild(0).gameObject;
        }

        private void Update() {
            if (trig == null || rend == null) { return; }

            offTime += Time.deltaTime;

            // Disable trigger and renderer during cooldown
            trig.enabled = rend.enabled = offTime >= cooldown;
        }

        private void OnTriggerEnter(Collider other) {
            if (manager != null) {
                // Give item to caster
                ItemCaster caster = other.transform.GetTopmostParentComponent<ItemCaster>();
                if (caster != null) {
                    if (AudioManagerNew.instance != null)
                    {
                        AudioManagerNew.instance.PlaySound("CoinPick");
                    }
                    offTime = 0.0f;

                    //// Give specific item if named, otherwise random item
                    ////caster.GiveItem(
                    ////    string.IsNullOrEmpty(itemName) ? manager.GetRandomItem() : manager.GetItem(itemName),
                    ////    ammo, false);

                    //MyChange
                    Item item = string.IsNullOrEmpty(itemName) ? manager.GetRandomItem() : manager.GetItem(itemName);
                    if (item.ItemCollectVfx)
                        Instantiate(item.ItemCollectVfx, transform.position, Quaternion.identity);

                    if (other.transform.root.CompareTag("Player"))
                        caster.GiveItemList(item, ammo, false, itemName);
                    else
                        caster.GiveItem(item, ammo, false);

                    if(vfx)
                        StartCoroutine(VfxControl());
                }
            }
        }

        private IEnumerator VfxControl()
        {
            vfx.SetActive(false);
            yield return new WaitForSeconds(cooldown);
            vfx.SetActive(true);

        }
    }
}