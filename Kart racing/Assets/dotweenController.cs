using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Required for DOTween
using DG.Tweening.Core;

public class dotweenController : MonoBehaviour
{
    private DOTweenAnimation[] animations;

    void Awake()
    {
        animations = GetComponents<DOTweenAnimation>();

        // Disable autoplay in case it's still enabled in Inspector
        foreach (var anim in animations)
        {
            anim.autoPlay = false;
            anim.DOPause();
        }
    }

    public void PlayAllAnimations()
    {
        if (animations.Length == 0) return;

        // Chain animations via code
        PlayAnimationAtIndex(0);
    }

    private void PlayAnimationAtIndex(int index)
    {
        if (index >= animations.Length) return;

        DOTweenAnimation currentAnim = animations[index];
        currentAnim.DORestart(); // Restart ensures it plays from the beginning

        currentAnim.onComplete.AddListener(() =>
        {
            // Unsubscribe to avoid stacking listeners on replay
            currentAnim.onComplete.RemoveAllListeners();
            PlayAnimationAtIndex(index + 1);
        });
    }
}
