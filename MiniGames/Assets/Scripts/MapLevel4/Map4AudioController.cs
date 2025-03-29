using UnityEngine;

public class Map4AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource backGroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backGroundAudioClip;
    [SerializeField] private AudioClip dogBarkingAudioClip;

    
    void Start()
    {
        PlayBackGroundMusic();
    }
    public void PlayBackGroundMusic()
    {
        backGroundAudioSource.clip = backGroundAudioClip;
        backGroundAudioSource.Play();
    }
    public void PlayDogBarkingSound()
    {
        effectAudioSource.PlayOneShot(dogBarkingAudioClip);
    }
}
