using LuckyThief.ThangScripts;
using UnityEngine;

public class MainLevelUi : MonoBehaviour
{
    [SerializeField] public GameManager gameManager;
    public void StartGame()
    {
        gameManager.RestartGame();
    }
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }
}
