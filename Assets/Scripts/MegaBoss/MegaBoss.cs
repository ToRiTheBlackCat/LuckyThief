using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class MegaBoss : Enemy
    {
        [SerializeField] private GameObject bulletPrefabs;
        [SerializeField] private GameObject misslePrefabs;
        [SerializeField] private GameObject lazerPrefabs;
        [SerializeField] private GameObject minionPrefabs;
        [SerializeField] private Transform firePoint;
        [SerializeField] private Transform misslePoint;
        [SerializeField] private Transform lazerPoint;
        [SerializeField] private float bulletSpeed = 30f;
        [SerializeField] private float missleSpeed = 20f;
        [SerializeField] private float skillCooldown = 4f;
        [SerializeField] private float hpHeal = 100f;
        [SerializeField] private BossAudioManager audioManager;
        private float nextSkillTime = 0f;


        protected override void Update()
        {
            base.Update();
            if (Time.time >= nextSkillTime)
            {
                UseSkill();
            }
            //if (Input.GetKeyUp(KeyCode.Space))
            //{
            //    SpawnMinion();
            //}
        }
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
            if (player != null)
            {
                animator.SetBool("isLazer", false);
                animator.SetBool("isHeal", false);
                animator.SetBool("isMissle", false);
                animator.SetBool("isAttack", true);
                Vector3 directionToPlayer = player.transform.position - misslePoint.position;
                directionToPlayer.Normalize();
                GameObject bullet = Instantiate(bulletPrefabs, misslePoint.position, Quaternion.identity);
                MegaBotAmmo megaBotAmmo = bullet.AddComponent<MegaBotAmmo>();
                megaBotAmmo.SetMovementDirection(directionToPlayer * missleSpeed);
                audioManager.PlayLaserSound();

            }

        }
        private void AttackMissle()
        {
            if (player != null)
            {
                animator.SetBool("isLazer", false);
                animator.SetBool("isHeal", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isMissle", true);
                int missleCount = 12;
                float angleStep = 360f / missleCount;
                for (int i = 0; i < missleCount; i++)
                {
                    float angle = i * angleStep;
                    Vector3 direction = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
                    GameObject missle = Instantiate(misslePrefabs, misslePoint.position, Quaternion.identity);
                    MegaBotAmmo megaBotAmmo = missle.AddComponent<MegaBotAmmo>();
                    megaBotAmmo.SetMovementDirection(direction * bulletSpeed);
                }
                audioManager.PlayMissleSound();
            }
        }

        private void AttackLazer()
        {
            if (player != null)
            {
                animator.SetBool("isMissle", false);
                animator.SetBool("isHeal", false);
                animator.SetBool("isAttack", false);
                animator.SetBool("isLazer", true);

                Vector3 directionToPlayer = player.transform.position - lazerPoint.position;
                directionToPlayer.Normalize();
                GameObject laser = Instantiate(lazerPrefabs, lazerPoint.position, Quaternion.identity);
                MegaBotAmmo megaBotMissle = laser.AddComponent<MegaBotAmmo>();
                megaBotMissle.SetMovementDirection(directionToPlayer * bulletSpeed);
                audioManager.PlayRoundBulletSound();
            }
        }
        private void Heal()
        {
            animator.SetBool("isMissle", false);
            animator.SetBool("isLazer", false);
            animator.SetBool("isAttack", false);
            animator.SetBool("isHeal", true);
            currentHP = Mathf.Min(currentHP + hpHeal, maxHP);
            UpdateHpBar();
            audioManager.PlayHealSound();
        }
        private void RandomSkill()
        {
            int randomSkill = Random.Range(0, 4);
            switch (randomSkill)
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    AttackMissle();
                    break;
                case 2:
                    AttackLazer();
                    break;
                case 3:
                    Heal();
                    break;
                case 4:
                    SpawnMinion();
                    break;
            }
        }

        private void SpawnMinion()
        {
            Instantiate(minionPrefabs, transform.position, Quaternion.identity);
        }
        protected override void Die()
        {
            animator.SetBool("isDead", true);
            animator.SetBool("isAttack", false);
            animator.SetBool("isMissle", false);
            animator.SetBool("isLazer", false);
            animator.SetBool("isHeal", false);
            audioManager.PlayRobotExplosion();
            base.Die();
        }
        private void UseSkill()
        {
            nextSkillTime = Time.time + skillCooldown;
            RandomSkill();
        }
    }
}
