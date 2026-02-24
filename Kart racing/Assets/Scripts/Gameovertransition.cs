using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameovertransition : MonoBehaviour
{
    public RectTransform[] rankingObjects;      // Drag & drop the RectTransforms of ranking UI objects
    public float moveDistance = 1000f;          // Distance to move out/in
    public float moveDuration = 0.5f;           // Time for each move
    public float delayBetweenItems = 0.1f;      // Delay between each element
    public float waitBeforeReturn = 2f;         // Delay before moving back in

    private Vector2[] originalPositions;

    // void Start()
    // {
    //     // Cache the original anchored positions
    //     originalPositions = new Vector2[rankingObjects.Length];
    //     for (int i = 0; i < rankingObjects.Length; i++)
    //     {
    //         originalPositions[i] = rankingObjects[i].anchoredPosition;
    //     }
    // }
    void OnEnable()
    {
        originalPositions = new Vector2[rankingObjects.Length];
        for (int i = 0; i < rankingObjects.Length; i++)
        {
            if (rankingObjects[i] != null)
            {
                originalPositions[i] = rankingObjects[i].anchoredPosition;
            }
            else
            {
                Debug.LogWarning($"rankingObjects[{i}] is null in OnEnable!");
            }
        }
    }

    // 🔘 Call this from the "Continue" button OnClick
    public void PlayRankingTransition()
    {
       
        StartCoroutine(AnimateOutThenIn());
    }

    IEnumerator AnimateOutThenIn()
    {
        Time.timeScale = 1f;
        // Animate out to the right
        for (int i = 0; i < rankingObjects.Length; i++)
        {
            yield return StartCoroutine(MoveToPosition(rankingObjects[i],
                originalPositions[i] + new Vector2(moveDistance, 0),
                moveDuration));
            yield return new WaitForSeconds(delayBetweenItems);
        }

        yield return new WaitForSeconds(waitBeforeReturn);

        // Animate in from the left (off-screen to left, then to final position)
        for (int i = 0; i < rankingObjects.Length; i++)
        {
            // Start off-screen left
            rankingObjects[i].anchoredPosition = originalPositions[i] - new Vector2(moveDistance, 0);

            // Move to final left-aligned position (original - 200 pixels)
            Vector2 finalLeftPos = originalPositions[i] - new Vector2(200f, 0);
            yield return StartCoroutine(MoveToPosition(rankingObjects[i], finalLeftPos, moveDuration));
            yield return new WaitForSeconds(delayBetweenItems);
        }
    }

    IEnumerator MoveToPosition(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 start = rect.anchoredPosition;
        float time = 0;

        while (time < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(start, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rect.anchoredPosition = target;
    }
}
