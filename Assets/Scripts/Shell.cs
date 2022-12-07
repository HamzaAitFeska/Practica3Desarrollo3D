using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shell : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Control:")]
    private bool m_Moving;
    public  float m_Distance;
    public  LayerMask m_Layers;
    public GameObject m_RayPoint;
    public NavMeshAgent m_Agent;

    // Unity Awake
    void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        m_Moving = false;
    }

    void Update()
    {
        if (!m_Moving)
            return;


        if (Physics.Raycast(new Ray(m_RayPoint.transform.position, this.transform.forward), m_Distance / 2.5f, m_Layers))
        {
            RotateShell();
        }
        
        if (!Physics.Raycast(new Ray(m_RayPoint.transform.position, -this.transform.up), m_Distance, m_Layers))
        {
            RotateShell();
        }

        
        m_Agent.destination = m_RayPoint.transform.position;

    }

    
    private void RotateShell()
    {
        m_Agent.enabled = false;

        this.transform.Rotate(new Vector3(0.0f, 180.0f + Random.Range(-25.0f, 25.0f), 0.0f));

        m_Agent.enabled = true;
    }

    

    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Goomba"))
        {
            other.GetComponent<Goomba>().KillPunch();
            m_Moving = true;
        }
        else if (other.CompareTag("Player"))
        {
            if (m_Moving)
            {
                other.GetComponent<PlayerLife>().DamagePlayer();
            }
            else
            {
                m_Moving = true;
                m_Agent.enabled = false;
                this.transform.rotation = Quaternion.Euler(other.transform.forward);
                m_Agent.enabled = true;
            }

        }
        else if (other.CompareTag("Koopa"))
        {
            other.GetComponent<Koopa>().KillPunch();
            m_Moving = true;
        }
    }
}
