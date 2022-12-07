using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour, IRestartGameElements
{
    // Start is called before the first frame update
    public static PlayerLife instance;
    [Header("PlayerLife Parameters")]
    private readonly int maxLife = 8;
    private int m_TotalLives = 3;
    public float currentLife;
    public KeyCode damagePlayer;
    public Vector3 CheckpointPosition;
    public Quaternion CheckpointRotation;
    public bool m_IsDead;
    public bool m_PlayedOnce;
    public bool m_IsCreated;
    public float m_TimetoComeback;
    public Text m_TotalLifesText;
    [Header("GameOver")]
    public GameObject GameOver;
    public GameObject UI;
    public GameObject ButtonRespawn;
    public float m_TimeToAppear;
    public bool m_HasAppeared;
    
    void Awake()
    {
        AudioController.instance.Play(AudioController.instance.backgroundMusic);
    }
    void Start()
    {
        instance = this;
        currentLife = maxLife;
        m_HasAppeared = false;
        m_IsCreated = false;
        m_IsDead = false;
        m_PlayedOnce = false;
        GameOver.SetActive(false);
        GameController.GetGameController().AddRestartGameElement(this);
    }


    // Update is called once per frame
    void Update()
    {
        m_TotalLifesText.text = m_TotalLives.ToString();
        if(currentLife <= 0 && !m_IsDead)
        {
            currentLife = 0;
            m_TotalLives--;
            m_IsDead = true;

            MarioPlayerController.instance.m_ActiveInput = false;
            CameraController.instance.m_AngleLocked = true;
            CameraController.instance.m_MouseLocked = true;

            MarioPlayerController.instance.GetComponent<Animator>().SetBool("Falling", false);
            MarioPlayerController.instance.GetComponent<CharacterController>().enabled = false;
            MarioPlayerController.instance.GetComponent<Animator>().SetBool("Die", true);

            if (uiManager.instance.isOutsideScreen == true)
            {
                uiManager.instance.m_AnimationUI.Play("HealthBarDown");
                uiManager.instance.isOutsideScreen = false;
            } 
        }
        if (m_IsDead)
        {
            m_TimeToAppear += Time.deltaTime;
            if (!m_HasAppeared && m_TimeToAppear >= 3f)
            {
                ShowGameOverScreen(m_TotalLives);
                m_HasAppeared = true;
            }
        }
        if (uiManager.instance.isOutsideScreen == false)
        {
            m_TimetoComeback += Time.deltaTime;
        }

        if (m_TimetoComeback > 3 && uiManager.instance.isOutsideScreen == false)
        {
            uiManager.instance.m_AnimationUI.Play("HealthBarUp");
            m_TimetoComeback = 0;
            uiManager.instance.isOutsideScreen = true;
        }

        if (Input.GetKeyDown(damagePlayer))
        {
            DamagePlayer();
        }
        
              
    }
    public void DamagePlayer()
    {
        m_TimetoComeback = 0;
        currentLife--;
        MarioPlayerController.instance.GetComponent<Animator>().SetTrigger("Hit");
        if (uiManager.instance.isOutsideScreen == true)
        {
            uiManager.instance.m_AnimationUI.Play("HealthBarDown");
            uiManager.instance.isOutsideScreen = false;
        }
    }

    public void Death()
    {
        StartCoroutine(Respawn());
    }
    public void AddHealth(int healthQuantity)
    {
        if (currentLife < maxLife)
        {
            currentLife += healthQuantity;
            if ((currentLife += healthQuantity) >= maxLife)
            {
                currentLife = maxLife;
            }
            else
            {
                currentLife += healthQuantity;
            }
            if (uiManager.instance.isOutsideScreen == true)
            {
                uiManager.instance.m_AnimationUI.Play("HealthBarDown");
                uiManager.instance.isOutsideScreen = false; 
            }
            m_TimetoComeback = 0;
        }
    }

    private IEnumerator Diee()
    {
        yield return new WaitForSeconds(3f);
        
        ShowGameOverScreen(m_TotalLives);
        if (currentLife > 0)
        {
            HideGameOverScreen();
        }
        
    }

    private IEnumerator Respawn()
    {
        transform.position = CheckpointPosition;
        yield return new WaitForSeconds(0.25f);
        HideGameOverScreen();
        MarioPlayerController.instance.GetComponent<CharacterController>().enabled = false;
        CameraController.instance.m_AngleLocked = false;
        CameraController.instance.m_MouseLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_IsCreated = true;
        m_IsDead = false;
        m_PlayedOnce = false;
        currentLife = maxLife;
    }
    public void ShowGameOverScreen(int m_TotalLives)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ButtonRespawn.SetActive(true);
        UI.SetActive(false);
        GameOver.SetActive(true);

        if (m_TotalLives <= 0)
        {
            AudioController.instance.Stop(AudioController.instance.backgroundMusic);
            AudioController.instance.PlayOneShot(AudioController.instance.gameoverMusic);
            ButtonRespawn.SetActive(false);
        }
    }
    public void HideGameOverScreen()
    {
        GameOver.SetActive(false);
        ButtonRespawn.SetActive(false);
        UI.SetActive(true);
    }
    public void RestartGame()
    {
        HideGameOverScreen();
        m_HasAppeared = false;
        MarioPlayerController.instance.m_ActiveInput = true;
        currentLife = maxLife;
        CameraController.instance.m_AngleLocked = false;
        CameraController.instance.m_MouseLocked = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        m_IsCreated = true;
        m_IsDead = false;
        m_PlayedOnce = false;
        m_TimeToAppear = 0;
        MarioPlayerController.instance.GetComponent<Animator>().SetBool("Die", false);
        if (uiManager.instance.isOutsideScreen == true)
        {
            uiManager.instance.m_AnimationUI.Play("HealthBarDown");
            uiManager.instance.isOutsideScreen = false;
        }
        CameraController.instance.m_Pitch = CameraController.instance.m_StartPitch;
        CameraController.instance.m_Yaw = CameraController.instance.m_StartYaw;    
    }

    public void GameRestart()
    {
        if (m_TotalLives > 0)
        {
            GameController.GetGameController().RestartGame();
        }
    }
    public void TryAgain()
    {
        m_TotalLives = 3;
        MarioPlayerController.instance.m_CurrentCheckPoint = null;
        GameController.GetGameController().RestartGame();
        AudioController.instance.Stop(AudioController.instance.gameoverMusic);
        AudioController.instance.Stop(AudioController.instance.backgroundMusic);
        AudioController.instance.Play(AudioController.instance.backgroundMusic);
    }
}
