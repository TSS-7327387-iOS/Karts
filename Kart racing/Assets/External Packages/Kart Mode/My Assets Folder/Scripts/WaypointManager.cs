using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PowerslideKartPhysics;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public int incrt = 0;
    public Transform[] wayPoints;
    public List<BasicWaypoint> basicWayPointList; 
    // Start is called before the first frame update

    private void Awake()
    {
        wayPoints = GetComponentsInChildren<Transform>();
        basicWayPointList.AddRange(GetComponentsInChildren<BasicWaypoint>());
    }

    void Start()
    {

        foreach (Transform item in transform)
        {
           item.GetComponent<BasicWaypoint>().ID = incrt++;
            if (incrt == transform.childCount)
            {
                item.LookAt(transform.GetChild(0));
            }
            else
                item.LookAt(transform.GetChild(incrt));
        }

      
    }
    // Method to get BasicWaypoint transform by ID
    public Transform GetWaypointTransformByID(int id)
    {
        BasicWaypoint waypoint = basicWayPointList.Find(wp => wp.ID == id);
        return waypoint != null ? waypoint.transform : null;
    }
}
