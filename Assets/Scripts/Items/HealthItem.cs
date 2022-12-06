using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    readonly int l_HealthToRestore = 1;

    public Animation m_Animation;
    public AnimationClip m_HealthItemIdleClip;
    public AnimationClip m_HealthItemPickedUpClip;
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
            SetHealthItemPickedUpAnimation();
            StartCoroutine(CoinPickedUp());
        }
    }
    public IEnumerator CoinPickedUp()
    {
        yield return new WaitForSeconds(m_HealthItemPickedUpClip.length);
        Destroy(gameObject);
    }
    void SetHealthItemIddleAnimation()
    {
        m_Animation.CrossFadeQueued(m_HealthItemIdleClip.name);
    }
    void SetHealthItemPickedUpAnimation()
    {
        m_Animation.Stop(m_HealthItemIdleClip.name);
        m_Animation.CrossFadeQueued(m_HealthItemPickedUpClip.name);
    }
}
