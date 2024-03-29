using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public Collider checkpointCollider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLife.instance.CheckpointPosition = transform.position;
            PlayerLife.instance.CheckpointRotation = MarioPlayerController.instance.GetComponent<CharacterController>().transform.rotation;
            MarioPlayerController.instance.m_CurrentCheckPoint = this.GetComponent<CheckPoint>();
            AudioController.instance.PlayOneShot(AudioController.instance.checkpointReached);
            checkpointCollider.enabled = false;
        }
    }
}
