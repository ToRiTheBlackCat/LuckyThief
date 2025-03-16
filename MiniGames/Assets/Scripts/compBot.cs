using UnityEngine;

public class compBot : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float chaseSpeed = 20f;
    public float chaseRange = 30f;
    public Transform player;
    public Animator animator;
    public Rigidbody2D rb;

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

        if(distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            StopChasing();
        }
    }

    void ChasePlayer()
    {
        animator.SetBool("isRunning", true);
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * chaseSpeed * Time.deltaTime);

        spriteRenderer.flipX = direction.x > 0;
    }

    void StopChasing()
    {
        animator.SetBool("isRunning", false);

    }
}
