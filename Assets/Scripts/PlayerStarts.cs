using UnityEngine;
using UnityEngine.UI;

public class PlayerStarts : MonoBehaviour
{
    public int m_ToTalStarts;
    int l_LastCheckStart;
    public static PlayerStarts instance;
    public Text m_NumberTextStarts;

    void Start()
    {
        m_ToTalStarts = 0;
        instance = this;
    }

    private void Update()
    {
        m_NumberTextStarts.text = m_ToTalStarts.ToString();
    }
    public void AddStart()
    {
        m_ToTalStarts++;

    }
}
