using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioSource backGroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;
    [SerializeField] private AudioClip buttonClickAudioClip;

    [SerializeField] TextMeshProUGUI volumeText;
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("volume", 0.6f);
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        if (backGroundAudioSource != null)
        {
            backGroundAudioSource.volume = volume;
        }
        PlayerPrefs.SetFloat("Volume", volume);
        volumeText.text = volume.ToString();
        volumeText.text = (volume * 10).ToString("0");
    }

    public void PlayClickButtonSound()
    {
        effectAudioSource.PlayOneShot(buttonClickAudioClip);
    }
}
