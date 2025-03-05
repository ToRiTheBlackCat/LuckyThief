using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager Instance;
    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;
    private bool isMinigameCompleted = false;
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

    // Update is called once per frame
    void Update()
    {
        
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
        isMinigameCompleted = false;
        SceneManager.LoadScene("MainLevel");
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
    public bool IsMinigameCompleted()
    {
        return isMinigameCompleted;
    }
    public void SetMinigameCompleted(bool value)
    {
        isMinigameCompleted = value;
    }

    public void SetMainLevelActive(bool active)
    {
        if (mainLevelObjects == null)
        {
            mainLevelObjects = SceneManager.GetSceneByName("MainLevel").GetRootGameObjects();
        }

        foreach (GameObject obj in mainLevelObjects)
        {
            obj.SetActive(active); // Bật/tắt tất cả object trong MainLevel
        }
    }
}
