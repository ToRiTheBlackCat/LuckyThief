using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float chaseSpeed = 25f; // Tốc độ khi phát hiện player
    [SerializeField] private float distance = 10f;
    [SerializeField] private float detectionRange = 15f; // Phạm vi phát hiện player
    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;
    private Vector3 startPos;
    private bool movingRight = true;
    private bool isChasing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player")?.transform; // Lấy Player theo tag
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
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
            Patrol();
        }
    }

    private void StartChasing()
    {
        isChasing = true;
        animator.SetBool("isAlert", isChasing);
    }

    private void StopChasing()
    {
        isChasing = false;
        animator.SetBool("isRunning", true);
        animator.SetBool("isAlert", isChasing);
    }

    private void ChasePlayer()
    {
        animator.SetBool("isRunning", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        // Xác định hướng quay theo trục X
        if ((direction.x > 0 && !movingRight) || (direction.x < 0 && movingRight))
        {
            movingRight = !movingRight;
            Flip();
        }
    }

    private void Patrol()
    {
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;

        animator.SetBool("isRunning", true);

        if (movingRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false;
                Flip();
            }
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true;
                Flip();
            }
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Prop"))
        {
            Debug.Log("Enemy gặp chướng ngại vật, quay lại!");

            // Đổi hướng di chuyển
            movingRight = !movingRight;
            Flip();
        }
    }
}
