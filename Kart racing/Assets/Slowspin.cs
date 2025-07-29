using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowspin : MonoBehaviour
{
    // Start is called before the first frame update
    public float spinSpeed = 20f;

    void Update()
    {
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
