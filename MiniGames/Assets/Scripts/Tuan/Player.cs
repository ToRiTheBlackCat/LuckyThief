using UnityEngine;
using UnityEngine.UI;
namespace LuckyThief.ThangScripts
{
    public class Player : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        // transform là 1 đối tượng thuộc lớp Transform
        // spriteRenderer là 1 đối tượng thuộc lớp SpriteRenderer
        public float moveSpeed = 10f;
        //public Vector3 moveInput;
        private Animator animator;
        private Rigidbody2D rb;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private BossGameManager bossGameManager;
        private SpriteRenderer spriteRenderer;
        [SerializeField] protected float maxHP = 100f;
        protected float currentHP;
        [SerializeField] private Image HPBar;
        [SerializeField] private BossAudioManager audioManager;
        [SerializeField] private audioManager mainLevelManager;
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
            if (gameManager != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    gameManager.PauseGame();
                }
            }
            if (bossGameManager != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    bossGameManager.PauseGame();
                }
            }
        }

        private void FixedUpdate()
        {
            HandleMovement();
            //UpdateAnimation();
        }
        void HandleMovement()
        {
            Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            rb.linearVelocity = playerInput.normalized * moveSpeed;
            if (playerInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (playerInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            if(playerInput != Vector2.zero)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }
        }
        //void UpdateAnimation()
        //{
        //    Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        //    rb.linearVelocity = playerInput.normalized * moveSpeed;
        //    if(playerInput)
        //    animator.SetBool("IsRunning", isRunning);
        //}

        public void TakeDamage(float damage)
        {
            currentHP -= damage;
            currentHP = Mathf.Max(currentHP, 0);
            UpdateHpBar();
            if (audioManager != null)
            {
                audioManager.PlayTakeDamagePlayer();
            }
            if(mainLevelManager != null)
            {
                mainLevelManager.PlayTakeDamagePlayer();
            }
            if (currentHP <= 0)
            {
                if (audioManager != null)
                {
                    audioManager.PlayDead();
                }
                if (mainLevelManager != null)
                {
                    mainLevelManager.PlayDead();
                }
                Die();

            }
        }

        public void Die()
        {
            //Destroy(gameObject);
            if (gameManager != null)
            {
                gameManager.GameOver();
            }
            if(bossGameManager != null)
            {
                bossGameManager.GameOver();
            }
        }

        private void UpdateHpBar()
        {
            if (HPBar != null)
            {
                HPBar.fillAmount = currentHP / maxHP;
            }
        }

    }
}
