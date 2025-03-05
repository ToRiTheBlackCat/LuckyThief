using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        // transform là 1 đối tượng thuộc lớp Transform
        // spriteRenderer là 1 đối tượng thuộc lớp SpriteRenderer
    public float moveSpeed = 2000f;
    public Vector3 moveInput;
    private Animator animator;
    private Rigidbody2D rb;
    private GameManager gameManager;
        // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindAnyObjectByType<GameManager>();
    }
        // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver())
        {
            return;
        }
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        rb.linearVelocity = moveInput.normalized * moveSpeed * Time.deltaTime;
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 0);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 0);
            }
        }
    }
    void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x)>0.1f || Mathf.Abs(moveInput.y) > 0.1f;
        animator.SetBool("IsRunning", isRunning);
    }

}
   
