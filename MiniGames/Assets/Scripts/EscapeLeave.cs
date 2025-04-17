using UnityEngine;

public class EscapeLeave : MonoBehaviour
{
    public float holdTimeRequired = 3f; // Time to hold the button to escape
    private float holdTime = 0f;
    private bool playerInZone = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            holdTime = 0f; // Reset hold time if player leaves
        }
    }

    void Update()
    {
        if (playerInZone && Input.GetKey(KeyCode.E)) // Change 'E' to any key or button
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdTimeRequired)
            {
                Debug.Log("Player Escaped!");
                // SceneManager.LoadScene("NextScene"); // Uncomment if you want to change scene
            }
        }
        else
        {
            holdTime = 0f; // Reset hold time if key is released
        }
    }
}
