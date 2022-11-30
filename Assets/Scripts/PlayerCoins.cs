using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerCoins : MonoBehaviour
{
    public int m_TotalCoins;
    int l_LastCheckCoins;
    public static PlayerCoins instance;
    public Text m_NumberTextCoins;

    void Start()
    {
        m_TotalCoins = 0;
        instance = this;
    }

    private void Update()
    {
        m_NumberTextCoins.text = m_TotalCoins.ToString();
    }
    public void AddCoin()
    {
        m_TotalCoins++;

    }
}
