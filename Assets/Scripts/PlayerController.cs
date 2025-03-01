using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 moveInput;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRD;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRD = GetComponent<SpriteRenderer>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        transform.position += moveInput * moveSpeed * Time.deltaTime;
        if (moveInput.x != 0)
        {
            if (moveInput.x > 0)
            {
                 spriteRD.flipX = false;

            }
            else
            {
                spriteRD.flipX = true;
            }
        }


    }
}



//using UnityEngine;

//public class DogController : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    private SpriteRenderer spriteRD;

//    public Animator dogAnimator;
//    public GameObject player;
//    public float maxDistance = 15f; // Decide your own max distance
//    public float speed = 2f;

//    private bool isTriggered = false;
//    private float distanceToPlayer = 0f;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        spriteRD = GetComponent<SpriteRenderer>();
//    }
//    private void OnTriggerEnter2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            isTriggered = true;
//            dogAnimator.SetBool("isTrigger", true);
//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("Player"))
//        {
//            // If you want the dog to immediately go back to sleep
//            isTriggered = false;
//            dogAnimator.SetBool("isTrigger", false);
//        }
//    }

//    void Update()
//    {
//        // Calculate distance
//        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

//        // Set animator float parameter
//        dogAnimator.SetFloat("distanceToPlayer", distanceToPlayer);

//        // If the dog is chasing, you might do movement here:
//        if (isTriggered)
//        {
//            // If distance is > maxDistance, stop chasing
//            if (distanceToPlayer > maxDistance)
//            {
//                isTriggered = false;
//                dogAnimator.SetBool("isTrigger", false);
//            }
//            else
//            {
//                // Move dog towards player
//                Vector2 direction = (player.transform.position - transform.position).normalized;
//                // Move using Rigidbody2D for smoother physics-based movement
//                rb.MovePosition(rb.position + direction * speed * Time.deltaTime);

//                if (direction.x > 0)
//                {
//                    spriteRD.flipX = true;
//                }
//                else
//                {
//                    spriteRD.flipX = false;
//                }
//            }
//        }
//        }
//}
