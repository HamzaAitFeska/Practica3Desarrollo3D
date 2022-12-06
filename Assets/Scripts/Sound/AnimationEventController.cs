using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    public ParticleSystem footStepDirt;
    public ParticleSystem footStepSmoke;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FootStepParticles()
    {
        footStepDirt.Play();
        footStepSmoke.Play();
    }
    void JumpNormalSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.marioSingleJump[Random.Range(0, AudioController.instance.marioSingleJump.Length)]);
    }
    void JumpDoubleSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.doubleJump);
    }
    void JumpTripleSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.marioTripleJump[Random.Range(0, AudioController.instance.marioTripleJump.Length)]);
    }
    void RandomizeStepSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.footSteps[Random.Range(0, AudioController.instance.footSteps.Length)]);
    }
    void PunchSingleSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.punchSingle);
    }
    void PunchDoulbeSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.punchDouble);
    }
    void KickSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.kick);
    }
    void MarioTakeDamageSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.marioTakeDamage);
    }
    void MarioDieSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.marioDies);
    }
}
