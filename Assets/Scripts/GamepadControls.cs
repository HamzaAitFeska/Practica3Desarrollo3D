using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadControls : MonoBehaviour
{
    PlayerControls gamepadControls;
    void Awake()
    {
        gamepadControls = new PlayerControls();

        gamepadControls.Gamplay.Jump.performed += ctx => PlayerJump();
    }
    void PlayerJump()
    {
        Debug.Log("JUMP");
    }
}
