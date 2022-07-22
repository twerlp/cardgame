using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovementButtons : MonoBehaviour
{
    PlayerInput playerInput;
    MoveVelocity playerVel;

    Vector2 dir; //player direction

    void Awake()
    {
        playerVel = GetComponent<MoveVelocity>();
        playerInput = GetComponent<PlayerInput>();
        PlayerControls controls = new PlayerControls();

        #region ControlMapping
        controls.Player.Move.performed += Move;
        controls.Player.Move.canceled += ctx => { dir.x = 0; dir.y = 0; };
        controls.Player.Move.Enable();
        #endregion
    }

    void Move(CallbackContext ctx) // Player walking (overworld)
    {
        dir = ctx.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        // Walking movement
        Vector2 velocity = new Vector2(dir.x * Time.fixedDeltaTime, dir.y * Time.fixedDeltaTime);
        playerVel.SetVelocity(velocity);
    }
}
