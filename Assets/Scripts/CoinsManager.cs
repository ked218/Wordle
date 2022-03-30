using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class CoinsManager : MonoBehaviour
{
    public CoinsSO coinsSO;

    public PuzzleState PuzzleState;

    ////This string will appear in the name of the player prefs
    //public String KEY_SAVE_COINS = "KEY_SAVE_COINS";

    public void Update()
    {
        ////Using key code to test
        if (Input.GetKey(KeyCode.Space))
            EarnCoin(20);
        //if (Input.GetKey(KeyCode.R))
        //    UseCoin(1);


        //When it complete so just plus the coins with the points
        if(PuzzleState == PuzzleState.Complete)
        {
            Debug.Log("Hi there!");
        }

    }

    public void EarnCoin(int amount)
    {
        coinsSO.coins += amount;
    }

    //public void EarnCoin(int amount)
    //{
    //    //Luu du lieu nhu Text nhung ho tro tren Unity - PlayerPrefs
    //    if (PlayerPrefs.HasKey(KEY_SAVE_COINS))
    //    {
    //        PlayerPrefs.SetInt(KEY_SAVE_COINS, PlayerPrefs.GetInt(KEY_SAVE_COINS) + amount);
    //    }
    //    else
    //    {
    //        PlayerPrefs.SetInt(KEY_SAVE_COINS, amount);
    //    }
    //}


    //Use coin method using scriptable object
    public void UseCoin(int amount)
    {
        if (coinsSO.coins > amount)
            coinsSO.coins -= amount;
    }


    //Use coid method using player prefs
    //public void UseCoin(int amount)
    //{
    //    PlayerPrefs.SetInt(KEY_SAVE_COINS, PlayerPrefs.GetInt(KEY_SAVE_COINS) - amount);
    //}
}
