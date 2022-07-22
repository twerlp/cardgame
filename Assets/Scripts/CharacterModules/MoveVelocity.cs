using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Vector2 velocityVector;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetVelocity(Vector2 velocityVector) {
        this.velocityVector = velocityVector;
    }

    private void FixedUpdate()
    {
        rb.velocity = velocityVector * moveSpeed;
    }
}
