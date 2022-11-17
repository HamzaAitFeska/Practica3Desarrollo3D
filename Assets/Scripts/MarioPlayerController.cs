using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MarioPlayerController : MonoBehaviour
{
    public Camera m_Camera;
    public float m_LerpRotation = 0.85f;
    public float m_WalkSpeed = 2.5f;
    public float m_RunSpeed = 6.5f;
    [Header("Jump")]
    public float m_VerticalSpeed;
    public bool m_OnGround = true;
    public float m_JumpSpeed = 5.0f;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public float m_AirTime;

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
        if (Input.GetKey(KeyCode.W))
        {
            l_HasMoved = true;
            l_Movement = l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.S))
        {
            l_HasMoved = true;
            l_Movement = -l_ForwardCamera;
        }
        if (Input.GetKey(KeyCode.A))
        {
            l_HasMoved = true;
            l_Movement -= l_RightCamera;
        }
        if (Input.GetKey(KeyCode.D))
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
        if (Input.GetMouseButtonDown(0))
        {
            m_Animator.SetTrigger("Punch");
        }
        m_characterController.Move(l_Movement);

        m_VerticalSpeed = m_VerticalSpeed + Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;

        if (Input.GetKeyDown(m_JumpKeyCode) && m_AirTime < 0.1f)
        {
            m_VerticalSpeed = m_JumpSpeed;
            m_Animator.SetTrigger("Jump");
        }
            

        CollisionFlags l_CollisionFlags = m_characterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_VerticalSpeed = 0.0f;
            m_OnGround = true;
            m_AirTime = 0;
        }
        else
        {
            m_AirTime += Time.deltaTime;
            m_OnGround = false;
        }
    }
}
