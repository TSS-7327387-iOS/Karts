using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyUi : MonoBehaviour {

	//
	//
	// public Text _coinsText;          // The text in main menu or UI
	// public GameController gameController;  // Reference for in-game Cointext
	//
	// void OnEnable()
	// {
	// 	CurrencyManager.coinsChangedEvent += UpdateCoinsUi;
	//
	// 	// 🟢 When UI is shown, pull current coins immediately
	// 	int currentCoins = CurrencyManager.instance.GetCoins();
	// 	UpdateCoinsUi(currentCoins);
	// }
	//
	// void OnDisable()
	// {
	// 	CurrencyManager.coinsChangedEvent -= UpdateCoinsUi;
	// }
	//
	// void UpdateCoinsUi(int newCoinsValue)
	// {
	// 	if (_coinsText != null)
	// 	{
	// 		_coinsText.text = newCoinsValue.ToString();
	// 	}
	//
	// 	if (gameController != null && gameController.Cointext != null)
	// 	{
	// 		gameController.Cointext.text = newCoinsValue.ToString();
	// 	}
	//
	// 	Debug.Log($"[CurrencyUi] Coins: {newCoinsValue}");
	// }
	public Text _coinsText; // shows current level coins in gameplay
	public GameController gameController;

	void OnEnable()
	{
		CurrencyManager.coinsChangedEvent += UpdateCoinsUi;
		UpdateCoinsUi(CurrencyManager.instance.GetLevelCoins());
	}

	void OnDisable()
	{
		CurrencyManager.coinsChangedEvent -= UpdateCoinsUi;
	}

	void UpdateCoinsUi(int newCoinsValue)
	{
		if (_coinsText != null)
		{
			_coinsText.text = newCoinsValue.ToString();
		}

		if (gameController != null && gameController.Cointext != null)
		{
			gameController.Cointext.text = newCoinsValue.ToString();
		}
	}
}
