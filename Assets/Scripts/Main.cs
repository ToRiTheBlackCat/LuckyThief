using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    static public Main Instance;
    public int switchCount;
    public GameObject winText;
    private int onCount = 0;

    private void Awake()
    {
        Instance = this;
    }
    public void SwitchChange(int points)
    {
        onCount = onCount + points;
        if(onCount == switchCount)
        {
            CompleteMinigame();
        }
    }

    private void CompleteMinigame()
    {
        winText.SetActive(true); // Hiển thị văn bản chiến thắng
        GameManager.Instance.SetMinigameCompleted(true);
        Invoke("ReturnToMainLevel", 1f); // Chuyển scene sau 1 giây (có thể điều chỉnh)
    }

    private void ReturnToMainLevel()
    {
        // Xóa scene minigame và kích hoạt lại MainLevel
        SceneManager.UnloadSceneAsync("Wire");
        GameManager.Instance.SetMainLevelActive(true);
    }
}
