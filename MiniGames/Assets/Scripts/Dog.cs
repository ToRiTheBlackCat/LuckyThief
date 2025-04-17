using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class Dog : MonoBehaviour
    {
        [SerializeField] public audioManager audioManager;
        public float moveSpeed = 20f;
        public float chaseSpeed = 25f;
        public float chaseRange = 30f;
        public Transform player;
        public Animator animator;
        private SpriteRenderer spriteRenderer;

        private Rigidbody2D rb;
        private bool isChasing = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // Kiểm tra các thành phần cần thiết
            if (rb == null || animator == null || spriteRenderer == null)
            {
                Debug.LogError("Missing required components on Dog!");
                enabled = false; // Tắt script nếu thiếu
                return;
            }

            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            if (player == null)
            {
                Debug.LogError("Player not found! Ensure Player has the correct tag.");
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Player player = collision.collider.GetComponent<Player>();
                if (player != null)
                {
                    player.TakeDamage(15);
                }
            }
        }
        void Update()
        {
            if (player == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                StartChasing();
            }
            else
            {
                StopChasing();
            }

            if (isChasing)
            {
                ChasePlayer();
            }
            else
            {
                StopChasing();
            }
        }

        private void StartChasing()
        {
            isChasing = true;
            animator.SetBool("isSleeping", false);
            animator.SetBool("isRunning", true);
        }

        private void StopChasing()
        {
            //audioManager.StopEffect();
            isChasing = false;
            animator.SetBool("isRunning", false);
            animator.SetBool("isSleeping", true);
        }

        void ChasePlayer()
        {
            audioManager.PlayDog();
            animator.SetBool("isWalking", false);
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime);

            // Lật sprite theo hướng
            spriteRenderer.flipX = direction.x > 0;
        }
    }
}