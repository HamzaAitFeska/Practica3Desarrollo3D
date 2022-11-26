using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMANAGER : MonoBehaviour
{
    // Start is called before the first frame update
    public Image m_FillHearth;
    public Text m_Power;
    public Animator m_AnimatorUI;
    public static UIMANAGER instance;
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHearthMario();
    }

    void UpdateHearthMario()
    {
        if(PlayerLife.instance.currentLife <=2)
        {
            m_FillHearth.color = Color.red;
            m_Power.color = Color.red;
        }
       
        if(PlayerLife.instance.currentLife == 3 || PlayerLife.instance.currentLife == 4)
        {
            m_FillHearth.color = Color.yellow;
            m_Power.color = Color.yellow;
        }

        if (PlayerLife.instance.currentLife == 5 || PlayerLife.instance.currentLife == 6)
        {
            m_FillHearth.color = Color.green;
            m_Power.color = Color.green;
        }

        if (PlayerLife.instance.currentLife == 7 || PlayerLife.instance.currentLife == 8)
        {
            m_FillHearth.color = Color.blue;
            m_Power.color = Color.blue;
        }
        m_FillHearth.fillAmount = PlayerLife.instance.currentLife / 8;
    }
}
