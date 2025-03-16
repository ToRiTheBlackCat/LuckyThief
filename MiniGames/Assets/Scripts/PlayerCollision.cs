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
 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Chest") && Input.GetKeyDown(KeyCode.E)) // Kiểm tra nếu va chạm với rương
        {
            //Destroy(collision.gameObject);
            nearbyChest = collision.gameObject; // Lưu rương gần nhất
            Debug.Log("Nhấn E để mở rương");
        }
        if (collision.collider.CompareTag("Enemy"))
        {
            gameManager.GameOver();
        }
    }
}
