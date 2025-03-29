using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    public static Dictionary<LevelNames, float> LevelResults = new Dictionary<LevelNames, float>();

    public Slider NoiseMeter;
    public Slider ProfitMeter;
    public Button RetryButton;
    public TextMeshProUGUI KeyFoundText;
    public GameObject GameOverScreen;
    [SerializeField] private Image _exitLabel;
    [SerializeField] private LevelNames selectedLevel;

    private bool isHoldingReset;
    private Color initColor;

    // Coroutines
    private Coroutine ExitHoldCoroutine;

    public enum LevelNames
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
        NoiseMeter.value = 0;
        ProfitMeter.value = 0;
        initColor = KeyFoundText.color;
        KeyFoundText.enabled = false;

        RetryButton.onClick.AddListener(() =>
        {
            var levelString = GetLevelString(selectedLevel);
            SceneManager.LoadScene($"Assets/Scenes/{levelString}.unity");
            Time.timeScale = 1f;
        });
    }

    private void Update()
    {
        if (_exitLabel.isActiveAndEnabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isHoldingReset = true;
                exitHold();
            }
        }

        if (Input.GetKeyUp(KeyCode.E) && isHoldingReset)
        {
            StopCoroutine(ExitHoldCoroutine ?? null);
            _exitLabel.fillAmount = 0f;
            isHoldingReset = false;
        }
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

    public void ShowKeyText()
    {
        KeyFoundText.color = initColor;
        KeyFoundText.enabled = true;

        var fadeDuration = 3f;
        var fadeTime = 0f;
        StartCoroutine(StartFade());

        IEnumerator StartFade()
        {
            yield return new WaitForSeconds(1.2f);
            while (fadeTime < fadeDuration)
            {
                var t = fadeTime / fadeDuration;

                KeyFoundText.color = Color.Lerp(initColor, Color.clear, t);
                fadeTime += 0.025f;
                yield return new WaitForSeconds(0.025f);
            }
            KeyFoundText.color = Color.clear;
            KeyFoundText.enabled = false;
        }
    }

    private void exitHold()
    {
        var exitDuration = 3f;
        var timePassed = 0f;

        if (ExitHoldCoroutine != null)
        {
            StopCoroutine(ExitHoldCoroutine);
        }
        ExitHoldCoroutine = StartCoroutine(StartExitHold());

        IEnumerator StartExitHold()
        {
            while (timePassed < exitDuration)
            {
                var t = timePassed / exitDuration;
                _exitLabel.fillAmount = t;

                yield return new WaitForSeconds(0.05f);
                timePassed += 0.05f;
            }
            SaveLevelResult();

            SceneManager.LoadScene($"Assets/Scenes/House3.unity");
            yield return null;
        }
    }

    public void SaveLevelResult()
    {
        var prevLevelValue = 0f;
        if (!LevelResults.TryGetValue(selectedLevel, out prevLevelValue))
        {
            LevelResults.Add(selectedLevel, ProfitMeter.value * 100f);
        }
        else if (prevLevelValue < ProfitMeter.value * 100)
        {
            LevelResults.Remove(selectedLevel);
            LevelResults.Add(selectedLevel, ProfitMeter.value * 100f);
        }
    }

    public void ItemValueUpdate(float normal)
    {
        ProfitMeter.value = normal;
    }
}


