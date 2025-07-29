using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIlookat : MonoBehaviour
{
    void LateUpdate()
    {
         transform.LookAt(transform.position + Camera.main.transform.rotation*Vector3.forward, Camera.main.transform.rotation*Vector3.up);
    }
}
