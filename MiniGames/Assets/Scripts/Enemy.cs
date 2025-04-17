using System;
using UnityEngine;
using UnityEngine.UI;
namespace LuckyThief.ThangScripts
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected float speed = 10f;
        [SerializeField] protected float maxHP = 50f;
        protected float currentHP;
        [SerializeField] private Image HPBar;
        private SpriteRenderer spriteRenderer;
        protected Player player;
        [SerializeField] protected float enterDamage = 10f;
        [SerializeField] protected float stayDamage = 2f;
        public Animator animator;
        protected virtual void Start()
        {
            animator = GetComponent<Animator>();
            player = FindAnyObjectByType<Player>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            currentHP = maxHP;
            UpdateHpBar();
        }

        protected virtual void Update()
        {
            MoveTowardsPlayer();
        }

        protected void MoveTowardsPlayer()
        {
            if (player != null)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                FlipEnemy();
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

        public virtual void TakeDamage(float damage)
        {
            currentHP -= damage;
            currentHP = Mathf.Max(currentHP, 0);
            UpdateHpBar();
            if (currentHP <= 0)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            Destroy(gameObject, 2f);
        }

        protected void UpdateHpBar()
        {
            if (HPBar != null)
            {
                HPBar.fillAmount = currentHP / maxHP;
            }
        }
    }
}
