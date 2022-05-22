using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D player;
    PlayerInput playerInput;

    Vector2 dir; //player direction

    public float speed; //player speed


    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
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
        player.velocity = new Vector2(dir.x * speed * Time.fixedDeltaTime * 50, dir.y * speed * Time.fixedDeltaTime * 50);
    }

}
