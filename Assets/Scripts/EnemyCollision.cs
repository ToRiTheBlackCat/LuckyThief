using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Enemy enemyScript;
    void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Prop"))
        {
            Debug.Log("Enemy gặp chướng ngại vật, quay lại!");

            // Đổi hướng di chuyển
            ////movingRight = !movingRight;
            //if (enemyScript != null)
            //{
            //    enemyScript.Flip();
            //}
        }
    }
}
