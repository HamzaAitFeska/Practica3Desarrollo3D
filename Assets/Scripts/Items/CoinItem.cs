using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CoinItem : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_CoinItemIddleClip;
    public AudioSource m_CoinItemPickup;
    void Start()
    {
        SetCoinItemIddleAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCoins.instance.AddCoin();
            AudioController.instance.PlayOneShot(m_CoinItemPickup);
            Destroy(gameObject);
        }

    }
    void SetCoinItemIddleAnimation()
    {
        m_Animation.CrossFadeQueued(m_CoinItemIddleClip.name);
    }
}
