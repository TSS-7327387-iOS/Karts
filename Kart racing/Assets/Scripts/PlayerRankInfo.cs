using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRankInfo : MonoBehaviour
{
    public Image position;
    public TextMeshProUGUI playerName, rank, score;
    
    public void UpdateInfo(Sprite pos,string pName,string rnk, string scr,float animationDelay)
    {
        position.sprite = pos;
        playerName.text = pName.ToUpper();
        rank.text = rnk.ToUpper();
        score.text = scr.ToUpper();
        transform.DOScale(1, 1).SetDelay(animationDelay).SetEase(Ease.OutExpo);
    }
}
