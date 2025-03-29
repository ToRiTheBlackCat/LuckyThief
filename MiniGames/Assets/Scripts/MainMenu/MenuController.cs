using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image levelButtonImage;
    public Sprite[] levelSprites;

    private string[] scenes = { "HouseLevel1", "Level2", "House3", "MapLevel4", "level5" }; 
    private int currentLevel = 0;
    public void NextLevel()
    {
        if (currentLevel < levelSprites.Length - 1)
        {
            currentLevel++;
            UpdateLevelUI();
        }
    }

    public void PreviousLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            UpdateLevelUI();
        }
    }
    private void UpdateLevelUI()
    {
        levelButtonImage.sprite = levelSprites[currentLevel];
        Debug.Log("level:" + currentLevel);
    }
    public void LoadLevel()
    {
        Debug.Log("load scene:" + scenes[currentLevel]);
        SceneManager.LoadScene(scenes[currentLevel]);
    }
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
