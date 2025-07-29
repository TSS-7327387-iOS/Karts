using System;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
//     public GameObject popup,notEnoughCoinsPopup,coinBadge;
//     public TextMeshProUGUI popupBuy;
//     public AudioSource popupSound;
//     public Image infoPic, infoName;
//     public TextMeshProUGUI infoTextSpeed, infoTextstrength, infoTextSA, infoTextDes;
//     public Transform selectionBtnParent;
//     public CharacterData[] spritesData;
//     public CharacterButton characterBtn;
//     CharacterButton[] btns;
//     public TextMeshProUGUI lockedInfoText;
//     public MainMenu menu;
//     public int[] playerPrices;
//     public GameObject congratxText;
//     public GameObject SquadClosedPopup;
//     private void Start()
//     {
//         btns = new CharacterButton[spritesData.Length];
//         for (int i = 0; i < spritesData.Length; i++)
//         {
//             //btns[i] = Instantiate(characterBtn, selectionBtnParent);
//             btns[i] = Instantiate(spritesData[i].characterBtn, selectionBtnParent);
//
//             if (PlayerPrefs.GetInt("PlayerRank") > i)
//             {
//                 btns[i].locked = false;
//                 print("index is"+i+" and ranks is"+ PlayerPrefs.GetInt("PlayerRank"));
//             }
//             btns[i].ID = i;
//             btns[i].UpdateData(spritesData[i].pic, spritesData[i].body, spritesData[i].arm, spritesData[i].name, spritesData[i].characterName, this);
//         }
//         foreach (var b in btns)
//         {
//             b.tick.SetActive(false);
//         }
//         btns[PlayerPrefs.GetInt("Player")].tick.SetActive(true);
//     }
//     
//     int characterIndex;
//
//     public void ShowCharacterInfo(int val)
//     {
//         //infoPic.sprite = spritesData[val].pic;
//         infoPic.sprite = spritesData[val].infoPic;
//
//
//         infoName.sprite = spritesData[val].name;
//         menu.UITouchedInactive();
//         infoTextDes.text = spritesData[val].Description.ToUpper();
//         infoTextSA.text = spritesData[val].SA.ToUpper();
//         infoTextSpeed.text = spritesData[val].speed.ToUpper();
//         infoTextstrength.text = spritesData[val].strength.ToUpper();
//         popupBuy.text = PlayerPrefs.GetInt("PlayerUnlocked"+val)==1 ? "SELECT" : playerPrices[val].ToString();
//         coinBadge.SetActive(PlayerPrefs.GetInt("PlayerUnlocked" + val) != 1);
//         popup.SetActive(true);
//         characterIndex = val;
//         PlayPopupSound();
//     }
//     public void PlayPopupSound()
//     {
//         popupSound.Play();
//     }
//     public void SelectCharacter()
//     {
//         // if(PlayerPrefs.GetInt("PlayerUnlocked" + characterIndex) == 1)
//         // {
//         //     CharacterSelected();
//         // }
//         // else
//         // {
//         //     if (playerPrices[characterIndex] <= PlayerPrefs.GetInt("Coin"))
//         //     {
//         //         PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin") - playerPrices[characterIndex]);
//         //         PlayerPrefs.SetInt("PlayerUnlocked" + characterIndex,1);
//         //         congratxText.SetActive(true);
//         //         CharacterSelected();
//         //     }
//         //     else
//         //     {
//         //         popup.SetActive(false);
//         //         notEnoughCoinsPopup.SetActive(true);
//         //     }
//         // }
//         
//         if (PlayerPrefs.GetInt("PlayerUnlocked" + characterIndex) == 1)
//         {
//             CharacterSelected();
//         }
//         else
//         {
//             int currentCoins = CurrencyManager.instance.GetSavedCoins();
//             int price = playerPrices[characterIndex];
//
//             if (price <= currentCoins)
//             {
//                 // Subtract coins using CurrencyManager
//                 CurrencyManager.instance.SubtractSavedCoins(price);
//                 //coinsText.text = CurrencyManager.instance.GetSavedCoins().ToString();
//
//                 MainMenu.inst.coinshowtext();
//                 
//                 PlayerPrefs.SetInt("PlayerUnlocked" + characterIndex, 1);
//                 congratxText.SetActive(true);
//                 CharacterSelected();
//             }
//             else
//             {
//                 popup.SetActive(false);
//                 notEnoughCoinsPopup.SetActive(true);
//             }
//         }
//         //SceneManager.LoadScene(1);
//     }
//
//     public void SquadClosed()
//     {
//         SquadClosedPopup.SetActive(true);
//     }
//     public void SquadClosedPopupOff()
//     {
//         SquadClosedPopup.SetActive(false);
//     }
//     
//     void CharacterSelected()
//     {
//         PlayerPrefs.SetInt("Player", characterIndex);
//         popup.SetActive(false);
//         foreach (var b in btns)
//         {
//             b.tick.SetActive(false);
//         }
//         btns[PlayerPrefs.GetInt("Player")].tick.SetActive(true);
//
//     }
//     public void CharacterUnlockingInfo(int id)
//     {
//         menu.UITouchedInactive();
//         lockedInfoText.text = spritesData[id].unlockingCriteria.ToUpper();
//         lockedInfoText.gameObject.SetActive(true);
//     }
// }
//
// [Serializable]
// public class CharacterData
// {
//     public Sprite infoPic;
//     public CharacterButton characterBtn;
//     public Sprite pic;
//     public Sprite body;
//     public Sprite arm;
//     public Sprite name;
//     public Sprite completeBodyCharacterpic;
//     public string characterName;
//     public string speed, strength, SA, Description;
//     public string unlockingCriteria;
//     public int buyingPrice;
// }
 public GameObject popup, notEnoughCoinsPopup, coinBadge;
    public TextMeshProUGUI popupBuy;
    public AudioSource popupSound;
    public Image infoPic, infoName;
    public TextMeshProUGUI infoTextSpeed, infoTextstrength, infoTextSA, infoTextDes;
    public Transform selectionBtnParent;
    public CharacterData[] spritesData;
    public CharacterButton characterBtn;
    CharacterButton[] btns;
    public TextMeshProUGUI lockedInfoText;
    public MainMenu menu;
    public int[] playerPrices;
    public GameObject congratxText;
    public GameObject SquadClosedPopup;

    private void Start()
    {
        btns = new CharacterButton[spritesData.Length];
        for (int i = 0; i < spritesData.Length; i++)
        {
            btns[i] = Instantiate(spritesData[i].characterBtn, selectionBtnParent);

            if (PlayerPrefs.GetInt("PlayerRank") > i)
            {
                btns[i].locked = false;
                print("index is" + i + " and rank is" + PlayerPrefs.GetInt("PlayerRank"));
            }
            btns[i].ID = i;
            btns[i].UpdateData(spritesData[i].completeBodyCharacterpic, spritesData[i].name, spritesData[i].characterName, this);
        }

        foreach (var b in btns)
        {
            b.tick.SetActive(false);
        }
        btns[PlayerPrefs.GetInt("Player")].tick.SetActive(true);
    }

    int characterIndex;

    public void ShowCharacterInfo(int val)
    {
        infoPic.sprite = spritesData[val].infoPic;
        infoName.sprite = spritesData[val].name;
        menu.UITouchedInactive();
        infoTextDes.text = spritesData[val].Description.ToUpper();
        infoTextSA.text = spritesData[val].SA.ToUpper();
        infoTextSpeed.text = spritesData[val].speed.ToUpper();
        infoTextstrength.text = spritesData[val].strength.ToUpper();
        popupBuy.text = PlayerPrefs.GetInt("PlayerUnlocked" + val) == 1 ? "SELECT" : playerPrices[val].ToString();
        coinBadge.SetActive(PlayerPrefs.GetInt("PlayerUnlocked" + val) != 1);
        popup.SetActive(true);
        characterIndex = val;
        PlayPopupSound();
    }

    public void PlayPopupSound() => popupSound.Play();

    public void SelectCharacter()
    {
        bool isUnlocked = PlayerPrefs.GetInt("PlayerUnlocked" + characterIndex) == 1;
        int currentCoins = CurrencyManager.instance.GetSavedCoins();
        int price = playerPrices[characterIndex];

        if (isUnlocked)
        {
            CharacterSelected();
        }
        else
        {
            // Player needs to buy
            if (price <= currentCoins)
            {
                // Deduct coins and unlock character
                CurrencyManager.instance.SubtractSavedCoins(price);
                MainMenu.inst.coinshowtext();

                PlayerPrefs.SetInt("PlayerUnlocked" + characterIndex, 1);
                congratxText.SetActive(true);
                CharacterSelected();
            }
            else
            {
                // Not enough coins
                popup.SetActive(false);
                notEnoughCoinsPopup.SetActive(true);
            }
        }
    }

    public void SquadClosed() => SquadClosedPopup.SetActive(true);
    public void SquadClosedPopupOff() => SquadClosedPopup.SetActive(false);

    void CharacterSelected()
    {
        PlayerPrefs.SetInt("Player", characterIndex);
        popup.SetActive(false);
        foreach (var b in btns)
            b.tick.SetActive(false);
        btns[PlayerPrefs.GetInt("Player")].tick.SetActive(true);
    }

    public void CharacterUnlockingInfo(int id)
    {
        menu.UITouchedInactive();
        lockedInfoText.text = spritesData[id].unlockingCriteria.ToUpper();
        lockedInfoText.gameObject.SetActive(true);
    }
}

[Serializable]
public class CharacterData
{
    public Sprite infoPic;
    public CharacterButton characterBtn;
    public Sprite completeBodyCharacterpic;
    public Sprite name;
    public string characterName;
    public string speed, strength, SA, Description;
    public string unlockingCriteria;
    public int buyingPrice;
}
