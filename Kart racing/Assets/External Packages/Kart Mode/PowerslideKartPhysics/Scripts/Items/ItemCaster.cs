// Copyright (c) 2023 Justin Couch / JustInvoke
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace PowerslideKartPhysics
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Kart))]
    // Class for using items, attached to kart
    public class ItemCaster : MonoBehaviour
    {
        Kart kart;
        Transform kartTr;
        Rigidbody kartRb;
        Collider kartCol;
        public Item item;
        public int ammo = 0;
        public float minCastInterval = 0.1f;
        float timeSinceCast = 0.0f;
        public UnityEvent castEvent;


        GameController gameController;


        private void Awake() {
            kart = GetComponent<Kart>();
            if (kart != null) {
                kartTr = kart.transform;
                kartRb = kart.GetComponent<Rigidbody>();
                if (kart.rotator != null) {
                    kartCol = kart.rotator.GetComponent<Collider>();
                }
            }

            gameController = FindAnyObjectByType<GameController>();
        }

        private void Update() {
            timeSinceCast += Time.deltaTime;
        }

        // Cast currently equipped item
        public void Cast() {
            if (item != null && kart != null && ammo > 0 && timeSinceCast >= minCastInterval) {
                if (kart.active && !kart.spinningOut) {
                    ammo = Mathf.Max(0, ammo - 1);
                    timeSinceCast = 0.0f;
                    ItemCastProperties props = new ItemCastProperties();
                    props.castKart = kart;

                    if (kartRb != null) {
                        props.castKartVelocity = kartRb.velocity;
                    }

                    props.castGravity = kart.currentGravityDir;
                    props.castPoint = kartTr.position;

                    if (kart.rotator != null) {
                        props.castRotation = kart.rotator.rotation;
                    }

                    props.castCollider = kartCol;
                    props.castDirection = kart.forwardDir;
                    item.Activate(props);
                    Debug.Log("item caster item name" + item.name);

                    // switch (item.name)
                    // {
                    //     case "StaticItem Health":
                    //         Debug.LogError("Health Gain Trigger");
                    //         kart.resetHealth();
                    //         break;
                    //   
                    //     default:
                    //         // code block
                    //         break;
                    // }
                    
                    if (item.name.Contains("StaticItem Health"))
                    {
                     //   Debug.LogError("Health Gain Trigger");
                        kart.resetHealth();
                    }
                    else
                    {
                        // default behavior
                    }
                    castEvent.Invoke();
                }
            }
        }

        // Equip the specified single-use item
        public void GiveItem(Item givenItem) {
            GiveItem(givenItem, 1, true);
        }

        // Equip the specified item with the ammo amount
        public void GiveItem(Item givenItem, int ammoCount) {
            GiveItem(givenItem, ammoCount, true);
        }

        // Equip the specified item with the ammo amount, overwriting currently equipped item if bypass is true
        public void GiveItem(Item givenItem, int ammoCount, bool bypass) {
            if (bypass || ammo == 0) {
                item = givenItem;
                ammo = ammoCount;
            }
        }



        //Equip and store multiple items in list
        public void GiveItemList(Item givenItem, int ammoCount, bool bypass, string itemName)
        {
            if (bypass || ammo == 0)
            {
                //Add Item in list If not exist
                if (!gameController.itemData.Any(item => item.itemName == itemName))
                {
                    AddItemInList(itemName, givenItem, ammoCount);
                    Debug.Log("GiveItemList called itemname" + itemName.ToString() +"item Type"+ givenItem.ToString());
                }
            }
           
        }

        private void AddItemInList(string name, Item itemType, int ammo)
        {
            ItemData newItem = new ItemData
            {
                itemName = name,
                _item = itemType,
                _ammo = ammo
            };

            gameController.itemData.Add(newItem);
            gameController.UpdatePowerUpUI(name, newItem);
             Debug.Log("AddItemInList called ItemData for ui =" + name.ToString() + "newItem =" + newItem.ToString());
        }

    }

    // Struct for passing item cast data
    public struct ItemCastProperties
    {
        public Kart castKart;
        public Kart[] allKarts;
        public Vector3 castKartVelocity;
        public Vector3 castPoint;
        public Quaternion castRotation;
        public Vector3 castDirection;
        public float castSpeed;
        public Vector3 castGravity;
        public Collider castCollider;
    }

    
}
