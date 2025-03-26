using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Theif : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;
    public Vector3 moveInput;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    public bool isCrouching = false;
    public bool isRunning = false;
    private bool isNearItem = false;
    public TextMeshProUGUI interactable;
    public InteractableController interactableController;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        rb.gravityScale = 0;
        interactable.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

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
        bool isMoving = moveInput.x != 0 || moveInput.y != 0;
        animator.SetBool("IsMoving", isMoving);
        if (isNearItem && Input.GetKeyDown(KeyCode.F))
        {
            interactableController.LoadSafeGame();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Minigames"))
        {
            Debug.Log("Item found!");
            isNearItem = true;
            if (interactable != null) interactable.gameObject.SetActive(true);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Minigames"))
        {
            Debug.Log("Item not found!");
            isNearItem = false;
            if (interactable != null) interactable.gameObject.SetActive(false);
        }
    }
}
