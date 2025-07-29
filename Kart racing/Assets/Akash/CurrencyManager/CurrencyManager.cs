using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour {

 
 
 public static CurrencyManager instance = null;
 
 public static event Action<int> coinsChangedEvent;
 public static event Action<int> gemsChangedEvent;
 
 private int levelCoins = 0;
 
 public string _coinsPref = "Coins";
 public string _gemsPref = "Crystals";
 
 [SerializeField] public int _CoinDefaultValue = 0;
 [SerializeField] public int _GemsDefaultValue = 0;
 
 void Awake()
 {
     if (!instance)
     {
         instance = this;
         DontDestroyOnLoad(this.gameObject);
     }
     else
     {
         Destroy(this.gameObject);
     }
 
     if (!PlayerPrefs.HasKey("firstTimeUpdate"))
     {
         PlayerPrefs.SetInt(_coinsPref, _CoinDefaultValue);
         PlayerPrefs.SetInt(_gemsPref, _GemsDefaultValue);
         PlayerPrefs.SetInt("firstTimeUpdate", 1);
     }
 }
 
 // 👇 Called when level begins
 public void ResetLevelCoins()
 {
     levelCoins = 0;
     coinsChangedEvent?.Invoke(levelCoins);
 }
 
 
 
 public void AddLevelCoin(int amount)
 {
     levelCoins += amount;
     coinsChangedEvent?.Invoke(levelCoins);
 }
 
 public int GetLevelCoins()
 {
     return levelCoins;
 }
 
 public int GetSavedCoins()
 {
     return PlayerPrefs.GetInt(_coinsPref, _CoinDefaultValue);
 }
 
 public void MergeCoins()
 {
     int saved = GetSavedCoins();
     int merged = saved + levelCoins;
 
     PlayerPrefs.SetInt(_coinsPref, merged);
     PlayerPrefs.Save();
 
     coinsChangedEvent?.Invoke(merged);
 }
 
 public void AddBonusCoins(int amount)
 {
     int current = GetSavedCoins();
     int newVal = current + amount;
     PlayerPrefs.SetInt(_coinsPref, newVal);
     PlayerPrefs.Save();
     coinsChangedEvent?.Invoke(newVal);
 }
 
 // Optional subtraction method if you want to subtract saved coins
 public void SubtractSavedCoins(int amount)
 {
     int current = GetSavedCoins();
     int newVal = Mathf.Max(0, current - amount);
     PlayerPrefs.SetInt(_coinsPref, newVal);
     PlayerPrefs.Save(); // <-- Add this line
     coinsChangedEvent?.Invoke(newVal);
 }
 
}
