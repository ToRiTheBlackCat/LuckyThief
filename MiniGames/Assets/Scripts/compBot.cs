using System;
using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class compBot : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public float chaseSpeed = 20f;
        public float chaseRange = 30f;
        public Transform player;
        public Animator animator;
        public Rigidbody2D rb;
        [SerializeField] public audioManager audioManager;

        private SpriteRenderer spriteRenderer;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        // Update is called once per frame
        void Update()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                StopChasing();
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
        protected void FlipEnemy()
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            Console.WriteLine(direction);
            if (player != null)
            {
                if (direction.x > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }
            }
        }

        void ChasePlayer()
        {
            audioManager.Playrobot();
            animator.SetBool("isRunning", true);
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * chaseSpeed * Time.deltaTime);

            FlipEnemy();
        }

        void StopChasing()
        {
            //audioManager.StopEffect();
            animator.SetBool("isRunning", false);

        }
    }
}
