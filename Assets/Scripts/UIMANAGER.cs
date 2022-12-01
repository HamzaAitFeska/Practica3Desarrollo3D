using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Image m_FillHearth;
    public Animation m_AnimationUI;
    public static uiManager instance;
    public bool isOutsideScreen = false;
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
        }
       
        if(PlayerLife.instance.currentLife > 2 && PlayerLife.instance.currentLife <= 4)
        {
            m_FillHearth.color = Color.yellow;
        }

        if (PlayerLife.instance.currentLife > 4 && PlayerLife.instance.currentLife <= 6)
        {
            m_FillHearth.color = Color.green;
        }

        if (PlayerLife.instance.currentLife  > 6 && PlayerLife.instance.currentLife <= 8)
        {
            m_FillHearth.color = Color.blue;
        }
        m_FillHearth.fillAmount = PlayerLife.instance.currentLife / 8;
    }
}
