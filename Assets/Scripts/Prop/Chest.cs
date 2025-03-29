//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class Chest : MonoBehaviour
    {
        public InteractiveController interactiveController;
        public static Chest Instance;
        private Animator animator;
        private bool isOpened = false;
        //private bool isMinigameCompleted = false;
        //private bool isCollision = false;
        private bool isImpact = false;
        private bool isTrigger = false;

        void Start()
        {
            animator = GetComponent<Animator>();
        }
        private void Update()
        {
            if (isImpact == true && isTrigger == true && Input.GetKeyUp(KeyCode.E))
            {
                LoadMinigame();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            isTrigger = true;
            isImpact = true;
        }
        public void LoadMinigame()
        {
            SceneManager.LoadSceneAsync("Wire", LoadSceneMode.Additive);
            GameManager.Instance.SetCurrentChest(this);
            isTrigger = false;
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
        //public void SetMinigameCompleted(bool value)
        //{
        //    isMinigameCompleted = value;
        //}
    }
}