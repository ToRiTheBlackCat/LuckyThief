using UnityEngine;

public class LockPickAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource startAudioSource;
    [SerializeField] private AudioSource backGroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    // LockPick Game
    [SerializeField] private AudioClip backGroundAudioClip;
    [SerializeField] private AudioClip pickStartAudioClip;
    [SerializeField] private AudioClip pickSuccessAudioClip;
    [SerializeField] private AudioClip pickFailAudioClip;
    [SerializeField] private AudioClip pickLong1AudioClip;
    [SerializeField] private AudioClip pickLong2AudioClip;
    [SerializeField] private AudioClip pickShortAudioClip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startAudioSource.PlayOneShot(pickStartAudioClip);
        PlayBackGroundMusic();
    }
    public void PlayBackGroundMusic()
    {
        backGroundAudioSource.clip = backGroundAudioClip;
        backGroundAudioSource.Play();
    }

    //LockPick Game
    public void PlayUnlockSound()
    {
        effectAudioSource.PlayOneShot(pickSuccessAudioClip);
    }
    public void PlayFailSound()
    {
        effectAudioSource.PlayOneShot(pickFailAudioClip);
    }
    public void PlayPickLong1Sound()
    {
        effectAudioSource.PlayOneShot(pickLong1AudioClip);
    }
    public void PlayPickShortSound()
    {
        effectAudioSource.PlayOneShot(pickLong2AudioClip);
    }
    public void PlayPickLong2Sound()
    {
        effectAudioSource.PlayOneShot(pickShortAudioClip);
    }

}
