using System;
using PowerslideKartPhysics;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class LapCounter : MonoBehaviour
{
    [HideInInspector]
    public int currentLap = 0;
    private int totalLap = 0;
    private Transform rotator;
    private bool hasCrossedFinishLine = false;
    private bool hasPassedMidpoint = false;
    private bool movingForward = true;
    [NonSerialized] public bool isPlayer = false;
    private bool raceStarted = false; // Prevent first crossing from counting
    GameController gameController;
    public int curntID;
    private Kart kart;
    private Rigidbody rb;
    
    public static List<string> finalRankList = new List<string>();

    private void Awake()
    {
        rotator = transform.GetChild(0);
        gameController = FindObjectOfType<GameController>();
    }

    void Start()
    {
        totalLap = gameController.levelData.totalLaps;
        kart = GetComponent<Kart>();
        rb = GetComponent<Rigidbody>();

        //Assign Way Ponits to Rank System
        waypoints = gameController.waypointManager.wayPoints;
    }
 

    public void CrossFinishLine(Collider other)
    {
        
       // Debug.LogError($"{gameObject.name} trying to cross finish line");
        movingForward = Vector3.Dot(other.transform.forward, rotator.forward) > 0.5f;
        

        // Ignore first crossing at the start
        if (!raceStarted)
        {
            raceStarted = true;
           // Debug.LogError($"{gameObject.name} started the race, initial crossing ignored");
            return;
        }
        

        if (!hasCrossedFinishLine && hasPassedMidpoint && movingForward)
        {
          // Debug.LogError($"{gameObject.name} has completed a lap");
            
            currentLap++;
            hasCrossedFinishLine = true;
            hasPassedMidpoint = false; // Reset for next lap

            

            if (isPlayer)
            {
                gameController.currentLapText.text = currentLap.ToString();
            }
           
            //Reset HasCrossedFinishLine bool untill max laps not complete
            if (currentLap < totalLap)
            {
                hasCrossedFinishLine = false;        
                //Debug.LogError($"{gameObject.name} reached lap");
            }
            else
            {
             //  Debug.LogError($"{gameObject.name} reached final lap, finishing...");
                //After complete laps stop Karts

                Invoke(nameof(StopKart), UnityEngine.Random.Range(0.5f, 3.5f));

                //Rank System
                CompleteLap();
                
                if (isPlayer)
                {
                    StartCoroutine(ShowFinishPanelAfterDelay());
                }

            }
        }
    }
    
    public IEnumerator ShowFinishPanelAfterDelay()
    {
        yield return new WaitForSeconds(6f);
        gameController.ShowFinishPanel();
    }

    private void StopKart()
    {
        kart.maxSpeed = 0f;
        kart.active = false;
        rb.drag = 2f;
    }

    public void PassMidpoint( Collider other)
    {
        movingForward = Vector3.Dot(other.transform.forward, rotator.forward) > 0.5f;

        if (movingForward)
        {
            hasPassedMidpoint = true;
          //  Debug.LogError($"{gameObject.name} passed midpoint");
        }
    }

    public void ResetLapProgress()
    {
        // If reversing after midpoint, reset progress
        hasPassedMidpoint = false;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponentInParent<KartAI>())
        {
            if (other.CompareTag("waypoint")){
                int Id = other.GetComponent<BasicWaypoint>().ID;
                curntID = Id;
            }
           
        }

        if (other.CompareTag("waypoint") && isPlayer)
        {
            movingForward = Vector3.Dot(other.transform.forward, rotator.forward) < 0f;

            int Id = other.GetComponent<BasicWaypoint>().ID;
            if(Id == gameController?.waypointManager.incrt-1 && curntID == 0 && movingForward)
            {
                gameController.wrongDirWarning.SetActive(true);
                //print("Wrong Direstion");
            }
            else if (curntID == gameController?.waypointManager.incrt - 1 && Id==0)
            {  
                curntID = Id;
                gameController.wrongDirWarning.SetActive(false);
               // print("Right Direstion");
            }
            else if ( Id < curntID && movingForward)
            {
                gameController.wrongDirWarning.SetActive(true);
               // print("Wrong Direstion");
            }
            else
            {
                //print("Right Direstion");
                curntID = Id;

                gameController.wrongDirWarning.SetActive(false);
            }
            Debug.Log("Curretn waypoint id " + curntID);
        }
    }

    public int currentRank;
    [HideInInspector]
    public bool hasFinished = false; // Track if race is completed
    [HideInInspector]
    public float finishTime; // Store when the car finished

    private float progressAlongTrack = 0f; // NEW: Progress along track

    string rankPostfix;

    void Update()
    {
        if (!hasFinished) // Only update distance if still racing
        {
            progressAlongTrack = GetTrackProgress();
        }

        if (isPlayer)
        {
            if(currentRank == 1)
            {
                rankPostfix = "st";
            }
            else if(currentRank == 2)
            {
                rankPostfix = "nd";
            }
            else if (currentRank == 3)
            {
                rankPostfix = "rd";
            }
            else
            {
                rankPostfix = "th";
            }

            //Only update when player start race
            if (raceStarted)
                gameController.currentPlayerRankText.text = currentRank.ToString() + rankPostfix;
 
        }
    }

    public float GetRaceProgress()
    {
        return currentLap * 10000f + progressAlongTrack;
    }

    public void CompleteLap()
    {
        // string kartName = isPlayer 
        //     ? PlayerPrefs.GetString("PlayerName", "Player") 
        //     : gameObject.name;
        //
        // if (!finalRankList.Contains(kartName))
        // {
        //     finalRankList.Add(kartName);
        // }
        
        string playerPrefName = PlayerPrefs.GetString("PlayerName", "").Trim();
        string kartName;

        if (isPlayer)
        {
            // If player name is empty or whitespace, use "Player"
            kartName = string.IsNullOrEmpty(playerPrefName) ? "Player" : playerPrefName;
        }
        else
        {
            // For enemy karts, clean out the (Clone) part
            kartName = gameObject.name.Replace("(Clone)", "").Trim();
        }

        if (!LapCounter.finalRankList.Contains(kartName))
        {
            LapCounter.finalRankList.Add(kartName);
        }
        
    }


    private Transform[] waypoints; // Assign waypoints in the Inspector
    private int currentWaypointIndex = 0;

    public float GetTrackProgress()
    {
        if (waypoints == null || waypoints.Length == 0)
            return 0f;

        // Find the closest waypoint
        float closestDistance = float.MaxValue;
        int closestIndex = currentWaypointIndex;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, waypoints[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        currentWaypointIndex = closestIndex;
        return closestIndex; // Return waypoint index as track progress

    }
}
