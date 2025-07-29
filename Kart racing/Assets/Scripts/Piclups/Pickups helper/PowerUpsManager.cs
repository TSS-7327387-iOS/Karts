using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PowerUpsManager : MonoBehaviour
{
    public GameObject[] powerups;
    public Transform[] randomSpots;
    [Tooltip("Landmin used for trapping ")]
    public GameObject Mine;
    public int totalPickups;
    private void Start()
    {
        var spots =randomSpots.ToList();
        for (int i=0;i<totalPickups;i++)
        {
            int index = Random.Range(0,powerups.Length);
            int spot = Random.Range(0,spots.Count);
            Instantiate(powerups[index], spots[spot].position,Quaternion.identity);
            spots.RemoveAt(spot);
        }
    }
}
