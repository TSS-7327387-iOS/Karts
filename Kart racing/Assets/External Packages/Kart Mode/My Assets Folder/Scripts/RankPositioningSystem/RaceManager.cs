using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    private List<LapCounter> karts = new List<LapCounter>();
    private List<LapCounter> finishedKarts = new List<LapCounter>();

    GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
        Invoke(nameof(Init), 0.3f);
    }

    private void Init()
    {
        foreach (GameObject kart in gameController.opponentKarts)
        {

            karts.Add(kart.GetComponent<LapCounter>());

        }
        karts.Add(gameController.player.GetComponent<LapCounter>());
    }

    void Update()
    {
        UpdateRanks();
    }

    void UpdateRanks()
    {
        // Separate finished and ongoing karts
        List<LapCounter> ongoingKarts = karts.Where(k => !k.hasFinished).ToList();

        // Sort finished karts by finish time
        finishedKarts.Sort((a, b) => a.finishTime.CompareTo(b.finishTime));

        // Sort ongoing karts by laps and track progress
        ongoingKarts.Sort((a, b) =>
        {
            //int lapComparison = b.lapCount.CompareTo(a.lapCount);
            int lapComparison = b.currentLap.CompareTo(a.currentLap);
            if (lapComparison != 0) return lapComparison;

            // Compare progress on the track
            return b.GetRaceProgress().CompareTo(a.GetRaceProgress());
        });

        // Merge both lists: Finished karts first, then ongoing karts
        List<LapCounter> rankedKarts = new List<LapCounter>();
        rankedKarts.AddRange(finishedKarts);
        rankedKarts.AddRange(ongoingKarts);

        // Assign ranks
        for (int i = 0; i < rankedKarts.Count; i++)
        {
            rankedKarts[i].currentRank = i + 1;
        }

    }

    public void KartFinishedRace(LapCounter kart)
    {
        if (!finishedKarts.Contains(kart))
        {
            finishedKarts.Add(kart);
            kart.finishTime = Time.time; // Store finish time
        }
    }
}