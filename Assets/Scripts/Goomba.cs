﻿using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;
using System;

public class Goomba : MonoBehaviour,IRestartGameElements
{
    public enum TSTATE
    {
        IDLE,
        PATROL,
        CHASE,
        ALERT,
        ATTACK,
        HIT,
        DIE
    }
    public GameObject BackWards;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 6.5f;
    public float m_KillTime = 0.5f;
    public float m_KillScale = 0.2f;
    public TSTATE m_State;
    NavMeshAgent m_NavMasAgent;
    public List<Transform> m_PatrolPoints;
    int CurrentPatrolID = 0;
    public float PlayerinRange = 2.0f;
    public float m_ShightMax = 10;
    public float m_VisualConeAngle = 60.0f;
    public LayerMask m_ShightLayerMask;
    public float m_CurrentRotationOnAlertedState;
    public float m_RotationSpeed = 150f;
    public float m_EyesPosition = 1.0f;
    public float m_PlayerEyesPosition = 1.0f;
    public float RangeToShootPlayer = 5f;
    Animator m_Animator;
    public AudioSource goombaDies;
    private void Start()
    {
        GameController.GetGameController().AddRestartGameElement(this);
        m_NavMasAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        SetIdleState();
    }

    private void Update()
    {
        switch (m_State)
        {
            case TSTATE.IDLE:
                UpdateIdleState();
                break;
            case TSTATE.PATROL:
                UpdatePatrolState();
                break;
            case TSTATE.ALERT:
                UpdateAlertState();
                break;
            case TSTATE.CHASE:
                UpdateChaseState();
                break;
            case TSTATE.ATTACK:
                UpdateAttackState();
                break;
            case TSTATE.HIT:
                UpdateHitState();
                break;
            case TSTATE.DIE:
                UpdateDieState();
                break;

        }
    }
    void SetIdleState()
    {
        m_State = TSTATE.IDLE;
    }

    void SetPatrolState()
    {
        m_State = TSTATE.PATROL;
        m_NavMasAgent.destination = m_PatrolPoints[CurrentPatrolID].position;
        m_NavMasAgent.speed = m_WalkSpeed;
        
    }

    void SetAlertState()
    {
        m_State = TSTATE.ALERT;
        m_NavMasAgent.destination = transform.position;
        m_CurrentRotationOnAlertedState = 0;
        m_Animator.SetBool("Alert", true);
        
    }
    void SetChaseState()
    {
        m_State = TSTATE.CHASE;
        m_NavMasAgent.speed = m_RunSpeed;
        
    }
    private void UpdateDieState()
    {
        
    }

    private void UpdateHitState()
    {
        
    }

    private void UpdateAttackState()
    {
        
    }

    private void UpdateChaseState()
    {
        MoveTowardsToPlayer();
    }

    private void UpdateAlertState()
    {
        float l_RotationSpeed = m_RotationSpeed * Time.deltaTime;
        m_CurrentRotationOnAlertedState += l_RotationSpeed;
        transform.Rotate(0, l_RotationSpeed, 0);
        if (SeePlayer())
        {
            SetChaseState();
        }
    }

    bool SeePlayer()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        Vector3 l_DirectionToPlayerXZ = l_PlayerPosition - transform.position;
        l_DirectionToPlayerXZ.y = 0;
        l_DirectionToPlayerXZ.Normalize();
        Vector3 l_ForwarXZ = transform.forward;
        l_ForwarXZ.y = 0;
        l_ForwarXZ.Normalize();
        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesPosition;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_PlayerEyesPosition;
        Vector3 l_Direction = l_PlayerEyesPosition - l_EyesPosition;
        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);
        float l_lenght = l_Direction.magnitude;
        l_Direction /= l_lenght;

        return Vector3.Distance(l_PlayerPosition, transform.position) < m_ShightMax && Vector3.Dot(l_ForwarXZ, l_DirectionToPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) && !Physics.Raycast(l_Ray, l_lenght, m_ShightLayerMask.value);
    }

    private void UpdatePatrolState()
    {
        if (PatrolTargetPosArrived())
        {
            //IsAlerted = false;
            MoveNextPatrolPoint();
        }

        if (HearsPlayer())
        {
            SetChaseState();
        }
        //SetIdleDronAnimation();
    }

    bool PatrolTargetPosArrived()
    {
        return !m_NavMasAgent.hasPath && !m_NavMasAgent.pathPending && m_NavMasAgent.pathStatus == NavMeshPathStatus.PathComplete;

    }

    void MoveNextPatrolPoint()
    {
        ++CurrentPatrolID;
        if (CurrentPatrolID == m_PatrolPoints.Count)
        {
            CurrentPatrolID = 0;
        }
        m_NavMasAgent.destination = m_PatrolPoints[CurrentPatrolID].position;

    }

    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= PlayerinRange && PlayerLife.instance.currentLife > 0; 
    }
    
    void MoveTowardsToPlayer()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        m_NavMasAgent.destination = Vector3.MoveTowards(transform.position, l_PlayerPosition, 1.0f);
    }

    bool PlayerInRangeToShoot()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= RangeToShootPlayer;
    }

    private void UpdateIdleState()
    {
        SetPatrolState();
    }

    public void Kill()
    {
        transform.localScale = new Vector3(1.0f, m_KillScale, 1.0f);
        AudioController.instance.PlayOneShot(goombaDies);
        StartCoroutine(Hide());
    }

    public void GoBackWards()
    {
        m_NavMasAgent.destination = Vector3.MoveTowards(transform.position, BackWards.transform.position , 1.0f);
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
