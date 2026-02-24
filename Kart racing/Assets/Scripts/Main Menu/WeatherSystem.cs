using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    [SerializeField] private GameObject vfx;
    float time;

    private void Start()
    {
        int rand = Random.Range(0, 2);

        if(rand == 1)
        {
            time = Random.Range(60, 140);

            StartCoroutine(Weather(time));
        }
        
    }


    IEnumerator Weather(float t)
    {
        vfx.SetActive(true);
        yield return new WaitForSeconds(t);
        vfx.SetActive(false);
    }
}
