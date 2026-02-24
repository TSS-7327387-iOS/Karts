using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class CharacterUnlock : MonoBehaviour
{

    public Image infoPic, infoName;
    public TextMeshProUGUI infoTextSpeed, infoTextstrength, infoTextSA, infoTextDes;
    public CharacterData[] spritesData;
    int characterIndex;
    public TextMeshProUGUI buy;
    public GameObject congratxPopup;
    public GameObject NotEnough, buyParent;

    public TextMeshProUGUI nameText;
    public void ShowCharacterInfo(int val)
    {
        //infoPic.sprite = spritesData[val].pic;
        infoPic.sprite = spritesData[val].completeBodyCharacterpic;
        //infoName.sprite = spritesData[val].name;
        nameText.text = spritesData[val].characterName;
        characterIndex = val;
        infoTextDes.text = spritesData[val].Description.ToUpper();
        infoTextSA.text = spritesData[val].SA.ToUpper();
        infoTextSpeed.text = spritesData[val].speed.ToUpper();
        infoTextstrength.text = spritesData[val].strength.ToUpper();
        buy.text = spritesData[val].buyingPrice.ToString() + " COINS";
    }
    public void BuyCharacter()
    {
        AudioManager.inst.UITouched();
        if (spritesData[characterIndex].buyingPrice <= PlayerPrefs.GetInt("Coin"))
        {
            PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - spritesData[characterIndex].buyingPrice);
            PlayerPrefs.SetInt("PlayerUnlocked" + characterIndex, 1);
            congratxPopup.SetActive(true);
            buyParent.SetActive(false);
        }
        else
        {
            NotEnough.SetActive(true);
        }

        //SceneManager.LoadScene(1);
    }
}
