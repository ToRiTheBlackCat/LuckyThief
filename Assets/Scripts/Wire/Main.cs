using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class Main : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        static public Main Instance;
        //public InteractiveController interactiveController;
        public int switchCount;
        public GameObject winText;
        private int onCount = 0;
        public UnityEvent onFinish;

        private void Awake()
        {
            Instance = this;
        }
        public void SwitchChange(int points)
        {
            onCount = onCount + points;
            if (onCount == switchCount)
            {
                CompleteMinigame();
            }
        }

        private void CompleteMinigame()
        {
            winText.SetActive(true); // Hiển thị văn bản chiến thắng
            SceneManager.UnloadSceneAsync("Wire");
            Chest currentChest = GameManager.Instance.GetCurrentChest();

            if (currentChest != null)
            {
                currentChest.OpenChest();
            }

            //Invoke("ReturnToMainLevel", 1f); // Chuyển scene sau 1 giây (có thể điều chỉnh)
        }

        private void ReturnToMainLevel()
        {
            // Xóa scene minigame và kích hoạt lại MainLevel
            //SceneManager.UnloadSceneAsync("Wire");
            //SceneManager.LoadSceneAsync("MainLevel");
            //GameManager.Instance.SetMainLevelActive(true);
        }
    }
}
