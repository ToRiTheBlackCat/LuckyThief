using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameObject nearbyChest;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>(); // Tìm GameManager trong scene
        if (gameManager == null)
        {
            Debug.LogError("GameManager không tìm thấy trong scene!");
        }
    }
    private void Update()
    {
        // Nếu đang gần rương và nhấn phím "E"
        if (nearbyChest != null && Input.GetKeyDown(KeyCode.E))
        {
            OpenChest();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Chest")) // Kiểm tra nếu va chạm với rương
        {
            //Destroy(collision.gameObject);
            nearbyChest = collision.gameObject; // Lưu rương gần nhất
            Debug.Log("Nhấn E để mở rương");
        }
        else if (collision.collider.CompareTag("Enemy"))
        {
            gameManager.GameOver();
        }
    }

    private void OpenChest()
    {
        if(nearbyChest != null)
        {
            Chest chestScript = nearbyChest.GetComponent<Chest>();
            if (chestScript != null)
            {
                chestScript.OpenChest();
            }
        }
    }
}
