using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private float damage = 15f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            Enemy enemy = collision.GetComponent<Enemy>();
            if (collision.CompareTag("Player"))
            {
                player.TakeDamage(damage);
            }
            if (collision.CompareTag("Enemy"))
            {
                enemy.TakeDamage(damage);
            }
        }
        public void DestroyExplosion()
        {
            Destroy(gameObject);
        }
    }
}
