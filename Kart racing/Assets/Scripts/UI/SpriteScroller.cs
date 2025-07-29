using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteScroller : MonoBehaviour
{
    [SerializeField] private float _x, _y;
    private Material _material;

    void Start()
    {
        _material = new Material(GetComponent<Image>().material);
        GetComponent<Image>().material = _material;
    }

    void Update()
    {
       
        Vector2 offset = _material.mainTextureOffset;
        offset += new Vector2(_x, _y) * Time.deltaTime;

        
        _material.mainTextureOffset = offset;
    }
}
