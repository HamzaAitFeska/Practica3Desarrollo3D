using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class PlayerCoins : MonoBehaviour
{
    public int m_TotalCoins;
    int l_LastCheckCoins;
    public static PlayerCoins instance;

    void Start()
    {
        m_TotalCoins = 0;
        instance = this;
    }
    public void AddCoin()
    {
        m_TotalCoins++;

    }
}
