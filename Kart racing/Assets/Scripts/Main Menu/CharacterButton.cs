using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CharacterButton : MonoBehaviour
{
    // public Image picture,body,arm, namePlate;
    // public TextMeshProUGUI nameText;
    // public GameObject cover;
    // public bool locked=true;
    // public CharacterSelection chS;
    // public int ID;
    // public GameObject tick;
    // private void Initialize()
    // {
    //     locked = false; //only for testing
    //     cover.SetActive(locked);
    //     GetComponent<Button>().interactable = !locked;
    //     GetComponent<Button>().onClick.AddListener(ShowPopup);
    //     float delay = Mathf.Clamp((float)ID / 4,0,11);
    //     transform.DOScale(1, 1).SetDelay(delay).SetEase(Ease.OutExpo);
    // }
    // public  void UpdateData(Sprite pic, Sprite bodypic, Sprite armpic, Sprite name, string _name,CharacterSelection cs)
    // {
    //     picture.sprite = pic;
    //     body.sprite = bodypic;
    //     arm.sprite = armpic;
    //     namePlate.sprite = name;
    //     nameText.text = _name;
    //     chS = cs;
    //     Initialize();
    // }
    // public void ShowPopup()
    // {
    //     chS.ShowCharacterInfo(ID);
    // }
    // public void ShowLockedInfo()
    // {
    //     chS.CharacterUnlockingInfo(ID);
    // }
    
    public Image picture, namePlate;
    public TextMeshProUGUI nameText;
    public GameObject cover;
    public bool locked = true;
    public CharacterSelection chS;
    public int ID;
    public GameObject tick;

    private void Initialize()
    {
       // cover.SetActive(locked);
        Button btn = GetComponent<Button>();
        btn.interactable = true;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(ShowPopup);

        float delay = Mathf.Clamp((float)ID / 4, 0, 11);
        transform.DOScale(1, 1).SetDelay(delay).SetEase(Ease.OutExpo);
    }

    public void UpdateData(Sprite pic, Sprite name, string _name, CharacterSelection cs)
    {
        picture.sprite = pic;

        // Set sprite to its native size
        picture.SetNativeSize();
        picture.rectTransform.anchoredPosition = Vector2.zero; // Center it in the button

        namePlate.sprite = name;
        nameText.text = _name;
        chS = cs;
        Initialize();
    }

    public void ShowPopup()
    {
        chS.ShowCharacterInfo(ID);
    }

    public void ShowLockedInfo()
    {
        chS.CharacterUnlockingInfo(ID);
    }
}
