using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartPlayerAnimatorController : MonoBehaviour
{
   [SerializeField] Animator animator;
    bool IsTurn = false;
    bool IsStand = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void PlayJumpAnimation()
    {
        if (!IsStand)
        {
            IsStand = true;
            animator.SetTrigger("Stand");
            StartCoroutine(PlayAnim());
        }
        
    }

    private IEnumerator PlayAnim()
    {
        yield return new WaitForSeconds(4f);
        IsStand = false;
    }

    public void PlayLeftRightAnimation(string para)
    {
        if (!IsTurn)
        {
            IsTurn = true;
            StartCoroutine(LeftRightAnim(para));
        }
        
    }

    private IEnumerator LeftRightAnim(string parameter)
    {
        animator.SetLayerWeight(1, 1f);
        animator.SetTrigger(parameter);
        yield return new WaitForSeconds(0.5f);
        animator.SetLayerWeight(1, 0f);
        IsTurn = false;
    }
}
