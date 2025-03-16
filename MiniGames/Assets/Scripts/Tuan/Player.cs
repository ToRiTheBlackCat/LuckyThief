using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
        // transform là 1 đối tượng thuộc lớp Transform
        // spriteRenderer là 1 đối tượng thuộc lớp SpriteRenderer
    public float moveSpeed = 10f;
    public Vector3 moveInput;
    private Animator animator;
    private Rigidbody2D rb;
    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;
    [SerializeField] protected float maxHP = 100f;
    protected float currentHP;
    [SerializeField] private Image HPBar;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindAnyObjectByType<GameManager>();
        currentHP = maxHP;
        UpdateHpBar();
    }
        // Update is called once per frame
    void Update()
    {
        //if (gameManager.IsGameOver())
        //{
        //    return;
        //}
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    void HandleMovement()
    {
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = playerInput.normalized * moveSpeed;
        if (playerInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(playerInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(moveInput.x)>0.1f || Mathf.Abs(moveInput.y) > 0.1f;
        animator.SetBool("IsRunning", isRunning);
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Max(currentHP, 0);
        UpdateHpBar();
        if (currentHP <= 0)
        {
            Die();
            
        }
    }

    public void Die()
    {
        Destroy(gameObject);
        gameManager.GameOver();
    }

    private void UpdateHpBar()
    {
        if (HPBar != null)
        {
            HPBar.fillAmount = currentHP / maxHP;
        }
    }

}
   
