using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    public Slider NoiseMeter;
    public Button RetryButton;
    public GameObject GameOverScreen;
    [SerializeField] private LevelNames selectedLevel;

    private enum LevelNames
    {
        Level1,
        Level2,
        Level3,
        Level4,
        Level5,
    }

    private void Start()
    {
        GameOverScreen.SetActive(false);

        RetryButton.onClick.AddListener(() =>
        {
            var levelString = GetLevelString(selectedLevel);
            SceneManager.LoadScene($"Assets/Scenes/{levelString}.unity");
            Time.timeScale = 1f;
        });
    }

    public void OnNoiseControllerNoiseChange(float normal)
    {
        NoiseMeter.value = normal;
    }

    public void OnNoiseControllerThreshold()
    {
        GameOverScreen.SetActive(true);

        Time.timeScale = 0f;
    }

    private string GetLevelString(LevelNames level)
    {
        string levelString = "";
        switch (level)
        {
            case LevelNames.Level1:
                levelString = "HouseLevel1";
                break;
            case LevelNames.Level2:
                levelString = "Level";
                break;
            case LevelNames.Level3:
                levelString = "House3";
                break;
            case LevelNames.Level4:
                levelString = "MapLevel4";
                break;
            case LevelNames.Level5:
                levelString = "";
                break;
        }

        return levelString;
    }
}


