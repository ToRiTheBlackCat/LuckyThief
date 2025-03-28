using UnityEngine;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class GameManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static GameManager Instance;
        [SerializeField] private GameObject gameOverUI;
        [SerializeField] private GameObject pauseGame;
        private bool isGameOver = false;
        private Chest currentChest;
        private GameObject[] mainLevelObjects;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Giữ GameManager qua các scene
            }
            else
            {
                Destroy(gameObject);
            }
        }
        void Start()
        {
            gameOverUI.SetActive(false);
            mainLevelObjects = SceneManager.GetSceneByName("MainLevel").GetRootGameObjects();
        }

        public void PauseGame()
        {
            Time.timeScale = 0;
            pauseGame.SetActive(true);
            gameOverUI.SetActive(false);
        }
        public void ResumeGame()
        {
            Time.timeScale = 1;
            pauseGame.SetActive(false);
            gameObject.SetActive(false);
        }

        public void GameOver()
        {
            isGameOver = true;
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
        }

        public void RestartGame()
        {
            isGameOver = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("MainLevel");
        }

        public bool IsGameOver()
        {
            return isGameOver;
        }
        public void SetCurrentChest(Chest chest)
        {
            currentChest = chest;
        }

        // Lấy rương hiện tại (dùng trong minigame)
        public Chest GetCurrentChest()
        {
            return currentChest;
        }
    }
}
