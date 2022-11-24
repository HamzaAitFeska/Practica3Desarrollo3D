using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    static GameController m_GameController = null;
    MarioPlayerController m_Mario;
    List<IRestartGameElements> m_RestartGameElements = new List<IRestartGameElements>();
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    public static GameController GetGameController()
    {
        if(m_GameController == null)
        {
            m_GameController = new GameObject("gamecontroller").AddComponent<GameController>();
            
        }
        return m_GameController;
    }

    public static void DestroySingleton()
    {
        if (m_GameController != null)
        {
            GameObject.Destroy(m_GameController.gameObject);
        }
        m_GameController = null;
    }
    public void AddRestartGameElement(IRestartGameElements RestartGameElement )
    {
        m_RestartGameElements.Add(RestartGameElement);
    }

    public void RestartGame()
    {
        foreach (IRestartGameElements l_RestartGameElement in m_RestartGameElements)
        {
            l_RestartGameElement.RestartGame();
        }
    }
    public void SetPlayer(MarioPlayerController Player)
    {
        m_Mario = Player;
    }
    public MarioPlayerController GetPlayer()
    {
        
        return m_Mario;
    }
    /*public void NextLevel()
    {
        LevelLoder.instance.LoadNextlevel(2);
    }*/
}
