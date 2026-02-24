using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpdater : MonoBehaviour
{
    public TextMeshProUGUI[] names,scoreText;
    public Image[] rankIcon;
    public Sprite[] rankSprites;
    public RectTransform[] scoreTextParents;
    public Vector2 Top,Mid,Bottom;
    [Tooltip("Only enter three(Top, Mid, Bottom)")]
    public Vector2[] positions;
    public float duration;
    bool nameFound=false;
    int index=0;

    public void DoMagic(int val, string name, int score)
    {
        index = 2;
        if (!scoreTextParents[val].gameObject.activeSelf)
            scoreTextParents[val].gameObject.SetActive(true);
        for (int i = 0; i < names.Length; i++)
        {
            if (name.ToUpper() == names[i].text)
            {
                nameFound = true;
                index = i;
                break;
            }
        }
        if (nameFound)
        {
            int topIndex, midIndex, bottomIndex;
            switch (val)
            {
                case 0:
                    topIndex = index;
                    scoreTextParents[topIndex].DOAnchorPos(Top, duration).SetEase(Ease.OutSine).onComplete =
                        () =>
                        {
                            rankIcon[topIndex].sprite = rankSprites[val];
                            //print(val+" rank of "+ name + " which was at:" + index);
                        };
                    break;
                case 1:
                    midIndex = index;
                    scoreTextParents[midIndex].DOAnchorPos(Mid, duration).SetEase(Ease.OutSine).onComplete =
                        () =>
                        {
                            rankIcon[midIndex].sprite = rankSprites[val];
                            // print(val + " rank of " + name + " which was at:" + index);
                        };
                    break;
                case 2:
                    bottomIndex = index;
                    scoreTextParents[bottomIndex].DOAnchorPos(Bottom, duration).SetEase(Ease.OutSine).onComplete =
                        () =>
                        {
                            rankIcon[bottomIndex].sprite = rankSprites[val];
                            // print(val + " rank of " + name + " which was at:" + index);
                        };
                    break;
            }
        }
        else
            index = val;

        nameFound = false;
        //scoreTextParents[i].DOLocalMove(positions[i], duration).SetEase(Ease.OutQuint);
        // scoreTextParents[index].SetSiblingIndex(val);
        names[index].text = name.ToUpper();
        if (score < 1000)
        {
            scoreText[index].text = score.ToString();
        }
        else
        {
            scoreText[index].text = Math.Round(score / 1000f, 2).ToString() + "K";
        }
    }
    public void UItouched()
    {
        AudioManager.inst.UITouched();
    }
    void SetRankIcon(int val)
    {
        rankIcon[index].sprite = rankSprites[val];
    }
}
