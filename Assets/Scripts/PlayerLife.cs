using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerLife instance;
    [Header("PlayerLife Parameters")]
    private readonly int maxLife = 8;
    private readonly int m_TotalLifes = 3;
    public float currentLife;
    public KeyCode damagePlayer;
    public Vector3 CheckpointPosition;
    public Quaternion CheckpointRotation;
    public bool m_IsDead;
    public bool m_PlayedOnce;
    public bool m_IsCreated;
    public float m_TimetoComeback;
    public Text m_TotalLifesText;
    /*[Header("GameOver")]
    public GameObject GameOver;
    public GameObject UI;*/
    
    void Start()
    {
        instance = this;
        currentLife = maxLife;
        m_IsCreated = false;
        m_IsDead = false;
        m_PlayedOnce = false;
        //GameOver.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_TotalLifesText.text = m_TotalLifes.ToString();
        /*if(currentLife <= 0)
        {
            currentLife = 0;
            GameOver.SetActive(true);
            CameraController.instance.m_AngleLocked = true;
            CameraController.instance.m_MouseLocked = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MarioPlayerController.instance.GetComponent<CharacterController>().enabled = false;
            UI.SetActive(false);
            m_IsDead = true;
        }*/

        if (UIMANAGER.instance.m_AnimatorUI.GetBool("DOWN") == true)
        {
            m_TimetoComeback += Time.deltaTime;
        }

        if (m_TimetoComeback > 3)
        {
            UIMANAGER.instance.m_AnimatorUI.SetBool("DOWN", false);
            UIMANAGER.instance.m_AnimatorUI.SetBool("UP", true);
            m_TimetoComeback = 0;
        }

        if (Input.GetKeyDown(damagePlayer))
        {
            DamagePlayer();
        }

              
    }

    private void LateUpdate()
    {
        if (m_IsDead && !m_PlayedOnce)
        {
            //AudioController.instance.PlayOneShot(AudioController.instance.playerDeath);
            m_PlayedOnce = true;
        }
    }

    public void DamagePlayer()
    {
        m_TimetoComeback = 0;
        currentLife--;
        UIMANAGER.instance.m_AnimatorUI.SetBool("DOWN", true);
        UIMANAGER.instance.m_AnimatorUI.SetBool("UP", false);
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
            UIMANAGER.instance.m_AnimatorUI.SetBool("DOWN", true);
            UIMANAGER.instance.m_AnimatorUI.SetBool("UP", false);
        }
    }

    private IEnumerator Respawn()
    {
        transform.position = CheckpointPosition;
        yield return new WaitForSeconds(0.25f);
        //GameOver.SetActive(false);
        //UI.SetActive(true);
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
}
