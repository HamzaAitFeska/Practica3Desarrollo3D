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
    void RandomizeStepSound()
    {
        AudioController.instance.PlayOneShot(AudioController.instance.footSteps[Random.Range(0, AudioController.instance.footSteps.Length)]);
    }
}
