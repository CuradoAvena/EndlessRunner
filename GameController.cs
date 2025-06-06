using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour

{
    public static GameController Instance { get; private set; }
    public int totalCoins;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddCoins(int amount)
    {
        totalCoins += amount;
        Debug.Log("Monedas: " + totalCoins);
    }
}
