using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class SummonEnemy : Enemy
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private BossAudioManager audioManager;
        private void CreateExplosion()
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
        }
        protected override void Die()
        {
            animator.SetBool("isDead", true);
            CreateExplosion();
            Destroy(gameObject);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CreateExplosion();
                audioManager.PlayExplosion();
                Die();
            }
        }
    }
}
