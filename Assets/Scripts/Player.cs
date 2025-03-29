using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 1.5f;
    public Vector3 moveInput;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    public bool isCrouching = false;
    public bool isRunning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
        }
        else if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            isCrouching = false;
        }
        else
        {
            isRunning = false;
            isCrouching = false;
        }

        if (moveInput.x != 0)
        {
            spriteRenderer.flipX = moveInput.x < 0;
        }
    }

    // FixedUpdate is called at a fixed interval and is independent of frame rate
    void FixedUpdate()
    {
        float currentSpeed = moveSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;

        Vector2 moveDirection = new Vector2(moveInput.x, moveInput.y).normalized;
        Vector2 newPosition = rb.position + moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }
}
