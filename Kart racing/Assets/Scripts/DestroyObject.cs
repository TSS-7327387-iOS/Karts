using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public int delay;
    void Start()
    {
        Destroy(gameObject,delay);
    }

    
}
