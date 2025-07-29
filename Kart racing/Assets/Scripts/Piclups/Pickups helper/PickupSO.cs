using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Pickable", menuName = "ScriptableObjects/Pickups")]
public class PickupSO : ScriptableObject
{
    public GameObject model;
    public Pickups pickupType;
    public float attaackAnimationBlendVal;
    public GameObject pickupProp;
}
