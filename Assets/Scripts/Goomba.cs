using UnityEngine;
using System.Collections;

public class Goomba : MonoBehaviour,IRestartGameElements
{
    public float m_KillTime = 0.5f;
    public float m_KillScale = 0.2f;
    private void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
    }
    public void Kill()
    {
        transform.localScale = new Vector3(1.0f, m_KillScale, 1.0f);
        StartCoroutine(Hide());
    }

    public void RestartGame()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        gameObject.SetActive(true);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(m_KillTime);
        gameObject.SetActive(false);
    }
    
}
