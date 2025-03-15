using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chest : MonoBehaviour
{
    public InteractiveController interactiveController;
    public static Chest Instance;
    private Animator animator;
    private bool isOpened = false;
    private bool isMinigameCompleted = false;
    private bool isCollision = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isCollision = true;
        }
    }

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player"))
    //    {
    //        isCollision = false;
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCollision == true && Input.GetKeyDown(KeyCode.E))
        {
            interactiveController.PlayWireGame();
            //GameManager.Instance.SetMainLevelActive(false);
            Debug.Log("Ïmpact");
            GameManager.Instance.SetCurrentChest(this);
        }
        else
        {
            Debug.Log("Not impact");
        }
    }

    public void OpenChest()
    {
        Debug.Log("Chest open");
        isOpened = true;
        animator.SetBool("isOpened", isOpened);
        GiveReward();
        Destroy(gameObject, 1f);
    }

    void GiveReward()
    {
        Debug.Log("Bạn đã nhận được phần thưởng!");
    }
    public void SetMinigameCompleted(bool value)
    {
        isMinigameCompleted = value;
    }
}