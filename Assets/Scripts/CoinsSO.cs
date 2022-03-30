using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObject/Coins", order = 1)]

public class CoinsSO : ScriptableObject
{
    public string objectName = "Coins SO";
    public int coins;

}
