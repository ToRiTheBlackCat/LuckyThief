using LuckyThief.ThangScripts;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public BossGameManager gameManager;
    public void StartGame()
    {
        gameManager.RestartGame();      
    }
    public void ResumeGame()
    {
        gameManager.ResumeGame();
    }

}
