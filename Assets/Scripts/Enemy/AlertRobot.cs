using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class AlertRobot : Enemy
    {
        [SerializeField] private float patrolDistance = 10f; // Khoảng cách tuần tra
        [SerializeField] private float detectionRange = 15f; // Phạm vi phát hiện player
        [SerializeField] private BossAudioManager audioManager;
        private Vector3 startPos; // Vị trí ban đầu để tuần tra
        private bool movingRight = true; // Hướng tuần tra
        private bool isChasing = false; // Trạng thái đuổi theo player

        protected override void Start()
        {
            base.Start(); // Gọi Start() của Enemy để lấy player và spriteRenderer
            startPos = transform.position; // Lưu vị trí ban đầu để tuần tra
        }

        protected override void Update()
        {
            // Kiểm tra khoảng cách đến player để quyết định tuần tra hay đuổi theo
            if (player != null && Vector2.Distance(transform.position, player.transform.position) < detectionRange)
            {
                isChasing = true;
                animator.SetBool("isAlert", isChasing);
                animator.SetBool("isRunning", !isChasing);
                audioManager.PlayAlert();
            }
            else
            {
                isChasing = false;
                animator.SetBool("isAlert", isChasing);
                animator.SetBool("isRunning", !isChasing);
                audioManager.StopAlert();
            }

            // Thực hiện hành vi tương ứng
            if (isChasing)
            {
                MoveTowardsPlayer(); // Sử dụng phương thức từ Enemy để đuổi theo
            }
            else
            {
                Patrol(); // Tuần tra khi không phát hiện player
            }
        }

        // Phương thức tuần tra từ code cũ
        private void Patrol()
        {
            float leftBound = startPos.x - patrolDistance;
            float rightBound = startPos.x + patrolDistance;
            animator.SetBool("isAlert", false);
            animator.SetBool("isRunning", true);

            if (movingRight)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
                if (transform.position.x >= rightBound)
                {
                    movingRight = false;
                    FlipEnemy(); // Sử dụng FlipEnemy từ Enemy để lật sprite
                }
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
                if (transform.position.x <= leftBound)
                {
                    movingRight = true;
                    FlipEnemy(); // Sử dụng FlipEnemy từ Enemy để lật sprite
                }
            }
        }
    }
}