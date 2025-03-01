using UnityEngine;

public class DogController : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer spriteRD;
    public Animator dogAnimator;
    public GameObject player;

    public float speed;
    public float maxDistance;
    public float minDistance;
    private float distanceToPlayer = 0f;

    private bool isTriggered = false;
    private bool isChasing = false;
    private bool isBarking = false; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRD = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            Debug.Log("Player detected!");
            isTriggered = true;
            dogAnimator.SetBool("isTrigger", true);
            isChasing = true;
        }
    }

    void Update()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        dogAnimator.SetFloat("distanceToPlayer", distanceToPlayer);

        if (isTriggered)
        {
            if (distanceToPlayer > maxDistance)
            {
                isTriggered = false;
                isChasing = false;
                isBarking = false;
                dogAnimator.SetBool("isTrigger", false);
            }
            else if (distanceToPlayer < minDistance)
            {
                DogBarking();
            }
            else
            {

                isBarking = false;
                dogAnimator.SetBool("isBarking", false);
                isChasing = true;
                DogChasing();
            }
        }
    }

    void DogChasing()
    {
        if (isBarking) return; // If barking, don't move

        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

        spriteRD.flipX = direction.x > 0;
        Debug.Log("Dog is chasing...");
    }

    private void DogBarking()
    {
        if (isBarking) return; // Prevent multiple calls to barking

        isBarking = true;
        dogAnimator.SetBool("isBarking", true);
        isChasing = false;

        rb.linearVelocity = Vector2.zero; // Stop movement
        Debug.Log("Dog Stopping and barking!");

        //Add barking audio for the dog
    }
}
