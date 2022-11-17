using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_LookAtTransform;
    public float m_MinxDistance = 5.0f;
    public float m_MaxDistance = 15.0f;
    public float m_YawRotationalSpeed = 720.0f;
    public float m_PitchRotationalSpeed = 720.0f;
    public float m_Pitch = 0.0f;
    public float m_MinPitch = -60.0f;
    public float m_MaxPitch = 20.0f;
    [Header("Avoid Obstacles")]
    public LayerMask m_AvoidObjectsMask;
    public float m_AvoidObjectOffset = 0.1f;
    [Header("Debug")]
    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    public bool m_AngleLocked = false;
    public bool m_MouseLocked = true;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        m_MouseLocked = Cursor.lockState == CursorLockMode.Locked;
    }
#if UNITY_EDITOR
    void UpadteInputDebug()
    {
        if (Input.GetKeyDown(m_DebugLockAngleKeyCode))
            m_AngleLocked = !m_AngleLocked;
        if (Input.GetKeyDown(m_DebugLockKeyCode))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            m_MouseLocked = Cursor.lockState == CursorLockMode.Locked;
        }
    }
#endif
    private void LateUpdate()
    {
#if UNITY_EDITOR
        UpadteInputDebug();
#endif
        float l_MouseX = Input.GetAxis("Mouse X");
        float l_MouseY = Input.GetAxis("Mouse Y");
#if UNITY_EDITOR
        if (m_AngleLocked)
        {
            l_MouseX = 0.0f;
            l_MouseY = 0.0f;
        }
#endif
        transform.LookAt(m_LookAtTransform.position);
        float l_Distance = Vector3.Distance(transform.position, m_LookAtTransform.position);
        l_Distance = Mathf.Clamp(l_Distance, m_MinxDistance, m_MaxDistance);
        Vector3 l_EulerAngles = transform.rotation.eulerAngles;
        float l_Yaw = l_EulerAngles.y;
        
        l_Yaw += l_MouseX * m_YawRotationalSpeed * Time.deltaTime;
        m_Pitch += l_MouseY * m_PitchRotationalSpeed * Time.deltaTime;
        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
        Vector3 l_ForwardCamera = new Vector3(Mathf.Sin(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad),
            Mathf.Sin(m_Pitch * Mathf.Deg2Rad), Mathf.Cos(l_Yaw * Mathf.Deg2Rad) * Mathf.Cos(m_Pitch * Mathf.Deg2Rad));
        Vector3 l_DesirePosition = m_LookAtTransform.position - l_ForwardCamera * l_Distance;


        Ray l_Ray = new Ray(m_LookAtTransform.position, -l_ForwardCamera);
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(l_Ray,out l_RaycastHit,l_Distance,m_AvoidObjectsMask.value))
        {
            l_DesirePosition = l_RaycastHit.point + l_ForwardCamera * m_AvoidObjectOffset;
        }


        transform.position = l_DesirePosition;
        transform.LookAt(m_LookAtTransform);

    }
}
