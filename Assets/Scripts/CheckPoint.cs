using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerLife.instance.CheckpointPosition = transform.position;
            PlayerLife.instance.CheckpointRotation = MarioPlayerController.instance.GetComponent<CharacterController>().transform.rotation;
            //AudioController.instance.PlayOneShot(AudioController.instance.checkpointReached);
            Destroy(gameObject);
        }
    }
}
