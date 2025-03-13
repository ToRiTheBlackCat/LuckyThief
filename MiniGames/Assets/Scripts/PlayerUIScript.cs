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


    private void Start()
    {
        GameOverScreen.SetActive(false);

        RetryButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Assets/Scenes/House3.unity");
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

        //Time.timeScale = 0f;
    }



}
