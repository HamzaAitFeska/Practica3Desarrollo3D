using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    readonly int l_HealthToRestore = 20;

    public Animation m_Animation;
    public AnimationClip m_HealthItemIddleClip;
    public AudioSource m_HealthItemPickup;
    private void Start()
    {
        SetHealthItemIddleAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLife.instance.AddHealth(l_HealthToRestore);
            AudioController.instance.PlayOneShot(m_HealthItemPickup);
            Destroy(gameObject);
        }
    }
    void SetHealthItemIddleAnimation()
    {
        m_Animation.CrossFadeQueued(m_HealthItemIddleClip.name);
    }
}
