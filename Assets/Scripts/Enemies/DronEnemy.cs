using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class DronEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public enum TSTATE
    {
        IDLE,
        PATROL,
        ALERT,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }
    public AudioSource droneIdle, droneAlert, droneShooting, droneDestroyed;
    private int RandomOption;
    public GameObject Dron;
    public GameObject [] RandomItem;
    public GameObject ItemPos;
    public TSTATE m_State;
    NavMeshAgent m_NavMasAgent;
    public List<Transform> m_PatrolPoints;
    int CurrentPatrolID = 0;
    public float PlayerinRange = 2.0f;
    public float m_ShightMax = 10;
    public float m_VisualConeAngle = 60.0f;
    public LayerMask m_ShightLayerMask;
    public float m_EyesPosition = 1.0f;
    public float m_PlayerEyesPosition = 1.0f;
    public float RangeToShootPlayer = 5f;
    public GameObject BulletPrefab;
    public bool IsAlerted;
    public float Dron_Life_MAX = 5;
    public float Dron_Current_Life;
    public float m_MaxShootDistance = 10.0f;
    public LayerMask m_ShootingLayerMask;
    public bool m_Shooting;
    public float TimeBetweenShots = 5f;
    public bool DronIsHit;
    public float m_CurrentRotationOnAlertedState;
    public float m_RotationSpeed = 150f;
    public static DronEnemy instacne;
    //bool m_IsCreated = false;
    [Header("UI")]
    public Image m_LifeBarImage;
    public Transform m_LifeAnchorPosition;
    public RectTransform m_LifeBarRectTransform;
    public GameObject lifebar;
    public Light lightdron;
    public Light Seelight;
    [Header("UI")]
    public Animation m_Animation;
    public AnimationClip m_IdleClip;
    public AnimationClip m_ShotClip;
    public AnimationClip m_HitdClip;
    public AnimationClip m_DieClip;
    private void Awake()
    {
        m_NavMasAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        SetIdleState();
        IsAlerted = false;
        Dron_Current_Life = Dron_Life_MAX;
        m_Shooting = false;
        m_LifeBarImage.fillAmount = Dron_Current_Life / Dron_Life_MAX;
        lifebar.SetActive(false);
        instacne = this;
        SetIdleDronAnimation();
        droneIdle.Play();

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
    private void LateUpdate()
    {
       UpdateLifeBarpOSITION();
    }

   void SetIdleState()
   {
        m_State = TSTATE.IDLE;
   }

    void UpdateIdleState()
    {
        SetPatrolState();
    }
    void SetPatrolState()
    {
        m_State = TSTATE.PATROL;
        m_NavMasAgent.destination = m_PatrolPoints[CurrentPatrolID].position;
        lightdron.color = Color.blue;
    }

    void UpdatePatrolState()
    {
        if (PatrolTargetPosArrived())
        {
            IsAlerted = false;
            MoveNextPatrolPoint();
        }

        if (HearsPlayer() )
        {
            SetAlertState();
        }
        SetIdleDronAnimation();
    }

    bool PatrolTargetPosArrived()
    {
        return !m_NavMasAgent.hasPath && !m_NavMasAgent.pathPending && m_NavMasAgent.pathStatus == NavMeshPathStatus.PathComplete;
        
    }

    void MoveNextPatrolPoint()
    {
        ++CurrentPatrolID;
        if(CurrentPatrolID == m_PatrolPoints.Count)
        {
            CurrentPatrolID = 0;
        }
        m_NavMasAgent.destination = m_PatrolPoints[CurrentPatrolID].position;

    }
    void SetAlertState()
    {
        m_State = TSTATE.ALERT;
        m_NavMasAgent.destination = transform.position;
        m_CurrentRotationOnAlertedState = 0;
        lightdron.color = Color.yellow;
    }

    void UpdateAlertState()
    {
        float l_RotationSpeed = m_RotationSpeed * Time.deltaTime;
        m_CurrentRotationOnAlertedState += l_RotationSpeed;
        transform.Rotate(0, l_RotationSpeed, 0);
        if (SeePlayer() && !PlayerInRangeToShoot())
        {
            SetChaseState();
        }

        if(SeePlayer() && PlayerInRangeToShoot())
        {
            SetAttackState();
        }

        if (!SeePlayer() && RotationComplete())
        {
            SetPatrolState();
        }


    }
    void SetHitState()
    {
        m_State = TSTATE.HIT;
        m_NavMasAgent.destination = transform.position;
        lightdron.color = Color.magenta;

        if (Dron_Current_Life <= 0)
        {
            SetDieState();
        }
    }

    void UpdateHitState()
    {
        SetHidDronAnimation();
    }
    void SetAttackState()
    {
        m_State = TSTATE.ATTACK;
        m_NavMasAgent.destination = transform.position;
        droneAlert.Play();
    }

    void UpdateAttackState()
    {
        lightdron.color = Color.red;
        if (CanShhot())
        {
           ShotDron();
        }

        if (!SeePlayer())
        {
            SetAlertState();
        }

        if(SeePlayer() & !PlayerInRangeToShoot())
        {
            SetChaseState();
        }

        if(PlayerLife.instance.currentLife <= 0)
        {
            SetPatrolState();
        }
                
    }
    void SetDieState()
    {
        SetDieDronAnimation();
        m_State = TSTATE.DIE;
        m_NavMasAgent.destination = transform.position;
        StartCoroutine(Die());
    }

    void UpdateDieState()
    {
        lifebar.SetActive(false);
    }
    void SetChaseState()
    {
        m_State = TSTATE.CHASE;
        lightdron.color = Color.red;
    }

    void UpdateChaseState()
    {
        MoveTowardsToPlayer();
        if(PlayerInRangeToShoot())
        {
            SetAttackState();
        }

        if (!SeePlayer())
        {
            SetAlertState();
        }

        
    }

    bool HearsPlayer()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= PlayerinRange && PlayerLife.instance.currentLife > 0; //&& FPSPlayerController.instance.m_PlayerIsMoving;
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
        Ray l_Ray = new Ray(l_EyesPosition,l_Direction);
        float l_lenght = l_Direction.magnitude;
        l_Direction /= l_lenght;

        return Vector3.Distance(l_PlayerPosition, transform.position) < m_ShightMax && Vector3.Dot(l_ForwarXZ, l_DirectionToPlayerXZ) > Mathf.Cos(m_VisualConeAngle * Mathf.Deg2Rad / 2.0f) && !Physics.Raycast(l_Ray, l_lenght, m_ShightLayerMask.value);
    }

    public void Hit(float Life)
    {
        Debug.Log(Life);
        Dron_Current_Life -= Life;
        DronIsHit = true;
        SetHitState();
        StartCoroutine(EndHit());
        m_LifeBarImage.fillAmount = Dron_Current_Life / Dron_Life_MAX;
        lifebar.SetActive(true);
    }

    void MoveTowardsToPlayer()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        m_NavMasAgent.destination = Vector3.MoveTowards(transform.position, l_PlayerPosition, 1.0f);
    }

    void ShotDron()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        Vector3 l_EyesPosition = transform.position + Vector3.up * m_EyesPosition;
        Vector3 l_PlayerEyesPosition = l_PlayerPosition + Vector3.up * m_PlayerEyesPosition;
        Vector3 l_Direction = l_PlayerPosition - l_EyesPosition;
        Ray l_Ray = new Ray(l_EyesPosition, l_Direction);
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray, out l_RaycastHit, m_MaxShootDistance, m_ShootingLayerMask))
        {
            CreatShootHitParticle(l_RaycastHit.collider, l_RaycastHit.point, l_RaycastHit.normal);
            PlayerLife.instance.DamagePlayer();
        }
        SetShootAnimation();
        droneShooting.Play();
        m_Shooting = true;
        StartCoroutine(EndShoot());
    }

    bool PlayerInRangeToShoot()
    {
        Vector3 l_PlayerPosition = MarioPlayerController.instance.transform.position;
        return Vector3.Distance(l_PlayerPosition, transform.position) <= RangeToShootPlayer;
    }

    void CreatShootHitParticle(Collider collider, Vector3 position, Vector3 Normal)
    {
        Debug.DrawRay(position, Normal * 5.0f, Color.red, 2.0f);
        

    }
    bool RotationComplete()
    {
        return m_CurrentRotationOnAlertedState >= 360;
    }

    
    public IEnumerator EndShoot()
    {
        yield return new WaitForSeconds(m_ShotClip.length);
        m_Shooting = false;
    }
    bool CanShhot()
    {
        return !m_Shooting;
    }

    public IEnumerator EndHit()
    {
        yield return new WaitForSeconds(m_HitdClip.length);
        DronIsHit = false;
        if (m_State != TSTATE.DIE) SetAlertState();
    }

    public IEnumerator Die()
    {
        droneIdle.Stop();
        droneDestroyed.Play();
        yield return new WaitForSeconds(m_DieClip.length);
        lightdron.color = Color.red;
        RandomOption = Random.Range(0, RandomItem.Length);
        Instantiate(RandomItem[RandomOption], ItemPos.transform.position, RandomItem[RandomOption].transform.rotation);
        Dron.SetActive(false);
        lightdron.intensity = 0;
        Seelight.intensity = 0;

    }

    void UpdateLifeBarpOSITION()
    {
        Vector3 l_Position = MarioPlayerController.instance.m_Camera.WorldToViewportPoint(m_LifeAnchorPosition.position);
        m_LifeBarRectTransform.anchoredPosition = new Vector3(l_Position.x * 1920.0f, -(1080.0f - l_Position.y * 1080.0f), 0.0f);
        m_LifeBarRectTransform.gameObject.SetActive(l_Position.z > 0.0f);
    }

    void SetIdleDronAnimation()
    {
        m_Animation.CrossFade(m_IdleClip.name);
    }

    void SetHidDronAnimation()
    {
        m_Animation.CrossFade(m_HitdClip.name, 0.05f);
        m_Animation.CrossFadeQueued(m_IdleClip.name, 0.05f);
    }

    void SetDieDronAnimation()
    {
        m_Animation.CrossFade(m_DieClip.name, 0.05f);
        m_Animation.CrossFadeQueued(m_IdleClip.name, 0.05f);
    }

    void SetShootAnimation()
    {
        m_Animation.CrossFade(m_ShotClip.name, 0.05f);
        m_Animation.CrossFadeQueued(m_IdleClip.name, 0.05f);
    }
}
