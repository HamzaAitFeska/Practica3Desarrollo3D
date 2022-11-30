using UnityEngine;

public class StarItem : MonoBehaviour
{
    public Animation m_Animation;
    public AnimationClip m_StartItemIddleClip;
    public AudioSource m_StartItemPickup;
    void Start()
    {
        SetStartItemIddleAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStarts.instance.AddStart();
            AudioController.instance.PlayOneShot(m_StartItemPickup);
            Destroy(gameObject);
        }

    }
    void SetStartItemIddleAnimation()
    {
        m_Animation.CrossFadeQueued(m_StartItemIddleClip.name);
    }
}
