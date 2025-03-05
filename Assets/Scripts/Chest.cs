using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private bool isOpened = false;
    private bool playerNearby = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerNearby && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            if (GameManager.Instance.IsMinigameCompleted())
            {
                OpenChest();
            }
            else
            {
                SceneManager.LoadSceneAsync("Wire", LoadSceneMode.Additive);
                GameManager.Instance.SetMainLevelActive(false);
                OpenChest();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = true;
            if (PlayerPrefs.GetInt("WireMinigameCompleted", 0) == 1)
            {
                Debug.Log("Nhấn E để mở rương");
            }
            else
            {
                Debug.Log("Nhấn E để chơi minigame Wire và mở rương");
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
    public void OpenChest()
    {
        isOpened = true;
        animator.SetBool("isOpened", isOpened);
        GiveReward();
        Destroy(gameObject,1f);
    }

    void GiveReward()
    {
        Debug.Log("Bạn đã nhận được phần thưởng!");
        // Thêm logic nhận phần thưởng (vàng, vật phẩm, v.v.)
    }
}
