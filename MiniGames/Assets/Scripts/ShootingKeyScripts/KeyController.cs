using UnityEngine;

public class KeyController : MonoBehaviour
{
    [SerializeField] float shootingSpeed = 1f;

    private bool isShooting = false;
    private bool isAttached = false; // Check if it's attached to Spin

    private GameController gameController;
    private ShootingKeyAudioController audioController;
    private void Awake()
    {
        gameController = FindAnyObjectByType<GameController>();
        audioController = FindAnyObjectByType<ShootingKeyAudioController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isShooting = false;
        isAttached = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAttached && Input.GetKeyDown(KeyCode.Space))
        {
            audioController.ShootingKeySound();
            isShooting = true;
        }

        if (isShooting && !isAttached)
        {
            transform.position += new Vector3(0, shootingSpeed, 0) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the key hits the rotating spin object
        if (collision.CompareTag("Spin"))
        {
            Debug.Log("Hit spin");
            audioController.HittingSpinSound();
            isShooting = false; // Stop moving up
            isAttached = true;   // Mark as attached

            transform.SetParent(collision.transform); // Attach to Spin

            if (gameController.keyNumber == 0)
            {
                gameController.GameWin();
            }
            else
            {
                // Notify GameController to create a new key
                gameController.SpawnNewKey();
            }
        }
        // If it hits another key
        else if (collision.CompareTag("Key"))
        {
            Debug.Log("Hit key");
            audioController.HittingKeySound();
            gameController.GameOver();
            return;
        }

    }
}
