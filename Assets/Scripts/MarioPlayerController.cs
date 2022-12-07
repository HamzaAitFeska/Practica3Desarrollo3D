using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MarioPlayerController : MonoBehaviour, IRestartGameElements
{
    public enum TPunchType { RightHand, LeftHand, Kick }
    public enum TJumpType { Jump, Double_Jump, Triple_Jump }
    [Header("Character Parameters")]
    public Camera m_Camera;
    public float m_LerpRotation = 0.85f;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 6.5f;
    public float m_TimeToIdleAnimation = 10.0f;
    float l_IdleTime = 0.0f;
    public static MarioPlayerController instance;
    public bool m_playerIsMoving;
    public bool m_ActiveInput;
    public GameObject GoRepulsion;
    Vector3 StartPosition;
    Quaternion StartRotation;
    [Header("Jump")]
    public float m_VerticalSpeed = 0.0f;
    bool m_OnGround = true; 
    public float m_JumpSpeed = 10.0f;
    public float m_JumpSpeedLong = 15.0f;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public float m_AirTime;
    public bool m_doubleJump = false;
    public bool m_tripleJump = false;
    public float m_TimeforLongJump = 0.5f;
    private float m_CurrentTimeButton;
    TJumpType m_CurrentJump;
    public float m_ComboJumpTime = 2.5f;
    float m_ComboJumpCurrentTime;
    [Header("Punch")]
    public float m_ComboPunchTime = 2.5f;
    float m_ComboPunchCurrentTime;
    public Collider m_LeftHandCollider;
    public Collider m_RightHandCollider;
    public Collider m_KickCollider;
    TPunchType m_CurrentPunch;
    bool m_IsPunchEnable = false;
    [Header("JumpKill")]
    public float m_JumpKillerSpeed = 5.0f;
    public float m_MaxAngleToKillGoomba = 60.0f;
    [Header("CheckPoint")]
    public CheckPoint m_CurrentCheckPoint = null;
    [Header("Elevator")]
    public float m_ElevatorDotAngle = 0.95f;
    Collider m_CurrentElevatorCollider = null;
    [Header("Bridge")]
    public float m_BridgeForce = 2.0f;
    [Header("Wall")]
    public bool m_AttachWall = false;
    [Header("AttachS hell")]
    public KeyCode m_PickObject = KeyCode.E;
    public Transform m_AttachingPosition;
    Rigidbody m_ObjectAttached;
    bool m_AttachingObject = false;
    Quaternion m_AttachingObjectStartRotation;
    public float m_AttachingObjectSpeed = 20f;
    public float m_MaxDistanceAttachObject = 10.0f;
    public LayerMask m_AttachObjectMask;
    public float m_ThrowAttachedObjectForce = 100f;
    CharacterController m_characterController;
    Animator m_Animator;
    // Start is called before the first frame update
    private void Awake()
    {
        m_characterController = GetComponent<CharacterController>();
        m_Animator = GetComponent<Animator>();
    }
    void Start()
    {
        m_playerIsMoving = false;
        m_ActiveInput = true;
        m_ComboJumpCurrentTime = -m_ComboJumpTime;
        m_ComboPunchCurrentTime =-m_ComboPunchTime;
        instance = this;
        m_LeftHandCollider.gameObject.SetActive(false);
        m_RightHandCollider.gameObject.SetActive(false);
        m_KickCollider.gameObject.SetActive(false);
        StartPosition = transform.position;
        StartRotation = transform.rotation;
        GameController.GetGameController().AddRestartGameElement(this);
        GameController.GetGameController().SetPlayer(this);
    }

    public void SetPunchActive(TPunchType PunchType,bool Active)
    {
        if(PunchType == TPunchType.RightHand)
        {
            m_RightHandCollider.gameObject.SetActive(Active);
        }
        else if (PunchType == TPunchType.LeftHand)
        {
            m_LeftHandCollider.gameObject.SetActive(Active);
        }
        else if(PunchType == TPunchType.Kick)
        {
            m_KickCollider.gameObject.SetActive(Active);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float l_Speed = 0.0f;
        Vector3 l_ForwardCamera = m_Camera.transform.forward;
        Vector3 l_RightCamera = m_Camera.transform.right;
        l_ForwardCamera.y = 0.0f;
        l_RightCamera.y = 0.0f;
        l_ForwardCamera.Normalize();
        l_RightCamera.Normalize();
        bool l_HasMoved = false;

        Vector3 l_Movement = Vector3.zero;
        if (Input.GetKey(KeyCode.W) && m_ActiveInput)
        {
            l_HasMoved = true;
            l_Movement = l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.S) && m_ActiveInput)
        {
            l_HasMoved = true;
            l_Movement = -l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.A) && m_ActiveInput)
        {
            l_HasMoved = true;
            l_Movement -= l_RightCamera;
        }
        if (Input.GetKey(KeyCode.D) && m_ActiveInput)
        {
            l_HasMoved = true;
            l_Movement += l_RightCamera;
        }

        l_Movement.Normalize();

        float l_MovementSpeed = 0.0f;

        if (l_HasMoved)
        {
            Quaternion l_LookRotation = Quaternion.LookRotation(l_Movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, l_LookRotation, m_LerpRotation);

            l_Speed = 0.5f;
            l_MovementSpeed = m_WalkSpeed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                l_Speed = 1.0f;
                l_MovementSpeed = m_RunSpeed;
            }
        }
        m_Animator.SetFloat("Speed", l_Speed);
        l_Movement = l_Movement * l_MovementSpeed * Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && CanPunch() && m_ActiveInput)
        {
            if (MustRestartComboPunch())
            {
                SetComboPunch(TPunchType.RightHand);
            }
            else
            {
                NextComboPunch();
            }
            
        }
        m_characterController.Move(l_Movement);

        if (Input.GetKeyUp(m_JumpKeyCode) && m_OnGround && Time.time - m_CurrentTimeButton < 2f && m_ActiveInput) //&& m_AirTime < 0.1f)
        {
            if(MustRestartComboJump())
            {
                m_VerticalSpeed = m_JumpSpeed;
                SetComboJump(TJumpType.Jump);
                l_HasMoved = true;
            }
            else
            {
                m_VerticalSpeed = m_JumpSpeed;
                NextComboJump();
                l_HasMoved = true;

            }
        }
        
        if (Input.GetKeyUp(m_JumpKeyCode) && m_OnGround && Time.time - m_CurrentTimeButton > 2f && m_ActiveInput)
        {
            m_VerticalSpeed = m_JumpSpeedLong;
            m_Animator.SetBool("LongJump", true);
            l_HasMoved = true;
        }

        if (Input.GetKeyDown(m_JumpKeyCode))
        {
            m_CurrentTimeButton = Time.time;
        }

        if (Input.GetKeyUp(m_JumpKeyCode))
        {
            Debug.Log((Time.time - m_CurrentTimeButton).ToString("00:00.00"));
            
        }


        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        CollisionFlags l_CollisionFlags = m_characterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
            m_AirTime = 0;
            m_Animator.SetBool("Falling", false);
            m_Animator.SetBool("Jump", false);
            m_Animator.SetBool("Jump2", false);
            m_Animator.SetBool("Jump3", false);
            m_Animator.SetBool("LongJump", false);
            //m_doubleJump = false;
            m_tripleJump = false;
        }
        else
        {
            m_AirTime += Time.deltaTime;
            //m_OnGround = false;
        }
        
        if(m_VerticalSpeed < 0 && m_AirTime > 0.1f)
        {
            m_Animator.SetBool("Falling", true);
            l_HasMoved = true;
        }

        if(m_VerticalSpeed > 0 && m_AirTime > 0.1f)
        {
            m_Animator.SetBool("Falling", false);
            l_HasMoved = true;
            m_OnGround = false;
        }
        m_playerIsMoving = l_HasMoved;

        if (!m_playerIsMoving)
        {
            l_IdleTime += Time.deltaTime;
        }
        SpecialIdleAnimation();

        if ((Input.GetKeyDown(m_PickObject)) && CanAttachObject())
        {
            AttachObject();
        }
        if (m_AttachingObject)
        {
            UpdateAttachObject();
        }

        if (!m_AttachingObject && m_ObjectAttached)
        {
            if (Input.GetMouseButtonDown(0))
            {
               ThrowAttachedObject(m_ThrowAttachedObjectForce);
            }
            if ((Input.GetMouseButtonDown(1) || (Input.GetKeyDown(m_PickObject))) && !CanAttachObject())
            {
                ThrowAttachedObject(0.0f);
            }

        }
    }

    private void LateUpdate()
    {
        if (m_CurrentElevatorCollider != null)
        {
            Vector3 l_EulerRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0.0f, l_EulerRotation.y, 0.0f);
        }
    }

    bool CanAttachObject()
    {
        return m_ObjectAttached == null;
    }

    void AttachObject()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit l_raycastHit;
        if (Physics.Raycast(l_Ray, out l_raycastHit, m_MaxDistanceAttachObject, m_AttachObjectMask.value))
        {
            if (l_raycastHit.collider.tag == "Shell")
            {
                m_AttachingObject = true;
                m_ObjectAttached = l_raycastHit.collider.GetComponent<Rigidbody>();
                m_ObjectAttached.GetComponent<Shell>().m_Agent.enabled = false;
                m_ObjectAttached.isKinematic = true;
                m_AttachingObjectStartRotation = l_raycastHit.collider.transform.rotation;
                
            }

        }
    }

    public void ThrowAttachedObject(float force)
    {
        if (m_ObjectAttached != null && m_ObjectAttached.tag == "Shell")
        {
            m_ObjectAttached.transform.SetParent(null);
            m_ObjectAttached.isKinematic = false;
            m_ObjectAttached.AddForce(transform.forward * force);
            m_ObjectAttached.GetComponent<Shell>().m_Agent.enabled = true;
            m_ObjectAttached = null;
            
        }

        

    }
    void UpdateAttachObject()
    {
        Vector3 l_EulerAngles = m_AttachingPosition.rotation.eulerAngles;
        Vector3 l_Direction = m_AttachingPosition.transform.position - m_ObjectAttached.transform.position;
        float l_Distance = l_Direction.magnitude;
        float l_Movement = m_AttachingObjectSpeed * Time.deltaTime;

        if (l_Movement >= l_Distance)
        {
            m_AttachingObject = false;
            m_ObjectAttached.transform.SetParent(m_AttachingPosition);
            m_ObjectAttached.transform.localPosition = Vector3.zero;
            m_ObjectAttached.transform.localRotation = Quaternion.identity;
            m_ObjectAttached.MovePosition(m_AttachingPosition.position);
            m_ObjectAttached.MoveRotation(Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z));
        }
        else
        {
            l_Direction /= l_Distance;
            m_ObjectAttached.MovePosition(m_ObjectAttached.transform.position + l_Direction * l_Movement);
            m_ObjectAttached.MoveRotation(Quaternion.Lerp(m_AttachingObjectStartRotation, Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z), 1.0f - Mathf.Min(l_Distance / 1.5f, 1.0f)));
        }


    }
    void SpecialIdleAnimation()
    {
        if (l_IdleTime > m_TimeToIdleAnimation)
        {
            m_Animator.SetTrigger("IdleExtra");
            l_IdleTime = 0;       
        }

        
    }
    void JumpOverEnemy()
    {
        m_VerticalSpeed = m_JumpKillerSpeed;
    }

    public void MoveBackWards(Goomba other)
    {
        var magnitude = 2.5f;
        var force = transform.position - other.transform.position;
        force.Normalize();
        m_characterController.Move(force * magnitude);
    }

    public void MoveBackWardsKoopa(Koopa other)
    {
        var magnitude = 2.5f;
        var force = transform.position - other.transform.position;
        force.Normalize();
        m_characterController.Move(force * magnitude);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Bridge")
        {
            hit.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }
        else if(hit.gameObject.tag == "Goomba")
        {
            if (CanKillGoomba(hit.normal))
            {
                hit.gameObject.GetComponent<Goomba>().Kill();
                JumpOverEnemy();
            }
            else
            {
                //Hacer Repulsion entre el Goomba y el Mario 
                MoveBackWards(hit.gameObject.GetComponent<Goomba>());
                hit.gameObject.GetComponent<Goomba>().GoBackWards(this);
                PlayerLife.instance.DamagePlayer();
                Debug.DrawRay(hit.point, hit.normal * 3.0f, Color.blue, 5.0f);
            }
        }
        else if(hit.gameObject.tag == "Koopa")
        {
            if (CanKillKoopa(hit.normal))
            {
                hit.gameObject.GetComponent<Koopa>().Kill();
                JumpOverEnemy();
            }
            else
            {
                //Hacer Repulsion entre el Goomba y el Mario 
                MoveBackWardsKoopa(hit.gameObject.GetComponent<Koopa>());
                hit.gameObject.GetComponent<Goomba>().GoBackWards(this);
                PlayerLife.instance.DamagePlayer();
                Debug.DrawRay(hit.point, hit.normal * 3.0f, Color.blue, 5.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DeadZone"))
        {
            PlayerLife.instance.currentLife = 0;
        }

        if(other.CompareTag("Elevator") && CanAttachtoElevator(other))
        {
            AttachToElevator(other);
        }

        if (other.CompareTag("Wall"))
        {
            AttachToWall(other);
            if (Input.GetKeyUp(m_JumpKeyCode) && m_AttachWall)
            {
                m_VerticalSpeed = m_JumpSpeed;
                transform.forward = other.transform.forward;
                m_ActiveInput = true;
                transform.SetParent(null);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Elevator"))
        {
            if (CanAttachtoElevator(other))
            {
                AttachToElevator(other);
            }
            if (m_CurrentElevatorCollider == other && Vector3.Dot(other.transform.up, Vector3.up) >= m_ElevatorDotAngle)
            {
                DetachElevator();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Elevator") && other == m_CurrentElevatorCollider)
        {
            DetachElevator();
        }

    }

    bool CanKillKoopa(Vector3 Normal)
    {
        return Vector3.Dot(Normal, Vector3.up) >= Mathf.Cos(m_MaxAngleToKillGoomba * Mathf.Deg2Rad);
    }

    bool CanKillGoomba(Vector3 Normal)
    {
        return Vector3.Dot(Normal, Vector3.up) >= Mathf.Cos(m_MaxAngleToKillGoomba * Mathf.Deg2Rad);
    }
    bool CanAttachtoElevator(Collider other)
    {
        return m_CurrentElevatorCollider == null && Vector3.Dot(other.transform.up, Vector3.up) <= m_ElevatorDotAngle;
    }

    void AttachToWall(Collider other)
    {
        transform.SetParent(other.transform);
        m_ActiveInput = false;
        m_AttachWall = true;
    }

    void AttachToElevator(Collider other)
    {
        transform.SetParent(other.transform);
        m_CurrentElevatorCollider = other;
    }

    void DetachElevator()
    {
        transform.SetParent(null);
        m_CurrentElevatorCollider = null;
    }
    bool CanPunch()
    {
        return !m_IsPunchEnable;
    }

    public void SetIsPunchEnable(bool IsPunchEnable)
    {
        m_IsPunchEnable = IsPunchEnable;
    }

    bool MustRestartComboPunch()
    {
        return (Time.time - m_ComboPunchCurrentTime) > m_ComboPunchTime;
    }

    void NextComboPunch()
    {
        if(m_CurrentPunch == TPunchType.RightHand)
        {
            SetComboPunch(TPunchType.LeftHand);
        }
        else if(m_CurrentPunch == TPunchType.LeftHand)
        {
            SetComboPunch(TPunchType.Kick);
        }
        else if(m_CurrentPunch == TPunchType.Kick)
        {
            SetComboPunch(TPunchType.RightHand);
        }
    }

    void SetComboPunch(TPunchType PunchType)
    {
        m_CurrentPunch = PunchType;
        m_ComboPunchCurrentTime = Time.time;
        m_IsPunchEnable = true;
        if(m_CurrentPunch == TPunchType.RightHand)
        {
            m_Animator.SetTrigger("Punch");
        }
        else if(m_CurrentPunch == TPunchType.LeftHand)
        {
            m_Animator.SetTrigger("PunchLeft");
        }
        else if(m_CurrentPunch == TPunchType.Kick)
        {
            m_Animator.SetTrigger("Kick");
        }

    }

    bool MustRestartComboJump()
    {
        return (Time.time - m_ComboJumpCurrentTime) > m_ComboJumpTime;
    }

    void NextComboJump()
    {
        if (m_CurrentJump == TJumpType.Jump)
        {
            SetComboJump(TJumpType.Double_Jump);
        }
        else if (m_CurrentJump == TJumpType.Double_Jump)
        {
            SetComboJump(TJumpType.Triple_Jump);
        }
        else if (m_CurrentJump == TJumpType.Triple_Jump)
        {
            SetComboJump(TJumpType.Jump);
        }
    }

    void SetComboJump(TJumpType JumpType)
    {
        m_CurrentJump = JumpType;
        m_ComboJumpCurrentTime = Time.time;
        m_IsPunchEnable = true;
        if (m_CurrentJump == TJumpType.Jump)
        {
            m_Animator.SetBool("Jump",true);
            m_Animator.SetBool("Jump3", false);
        }
        else if (m_CurrentJump == TJumpType.Double_Jump)
        {
            m_Animator.SetBool("Jump", false);
            m_Animator.SetBool("Jump2", true);
        }
        else if (m_CurrentJump == TJumpType.Triple_Jump)
        {
            m_Animator.SetBool("Jump2", false);
            m_Animator.SetBool("Jump3", true);
        }

    }

    public void RestartGame()
    {
        m_characterController.enabled = false;
        if(m_CurrentCheckPoint == null)
        {
            transform.position = StartPosition;
            transform.rotation = StartRotation;
        }
        else
        {
            transform.position = PlayerLife.instance.CheckpointPosition;
            transform.rotation = PlayerLife.instance.CheckpointRotation;
        }
        m_characterController.enabled = true;
        
    }
}
