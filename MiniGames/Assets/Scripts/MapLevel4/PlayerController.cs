using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 moveInput;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRD;

    public TextMeshProUGUI interactionHint;
    private bool isNearItem = false;

    private InteractableController interactableController;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRD = GetComponent<SpriteRenderer>();
        interactableController = FindAnyObjectByType<InteractableController>();

        if (interactionHint != null)
        {
            interactionHint.gameObject.SetActive(false);
        }
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
        if (interactableController.isSuccess == false)
        {
            if (isNearItem && Input.GetKeyDown(KeyCode.F))
            {
                interactableController.LoadGame();
                interactionHint.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Debug.Log("Item found");
            isNearItem = true;
            if (interactionHint != null)
            {
                interactionHint.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            isNearItem = false;
            if (interactionHint != null)
            {
                interactionHint.gameObject.SetActive(false);
            }
        }
    }


}



