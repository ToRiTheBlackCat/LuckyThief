using UnityEngine;

public class MegaBoss : Enemy
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(enterDamage);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(stayDamage);
            }
        }
    }

    private void Attack()
    {

    }
    private void AttackMissle()
    {

    }

    private void AttackLazer()
    {

    }
    private void Shield()
    {

    }
}
