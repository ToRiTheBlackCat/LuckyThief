using UnityEngine;

public class ShootingKeyAudioController : MonoBehaviour
{
    [SerializeField] private AudioSource backGroundAudioSource;
    [SerializeField] private AudioSource spinningAudioSource;
    [SerializeField] private AudioSource effectAudioSource;


    // ShootingKey Game
    [SerializeField] private AudioClip backGroundAudioClip;
    [SerializeField] private AudioClip SpiningAudioClip;
    [SerializeField] private AudioClip ShootingKeyAudioClip;
    [SerializeField] private AudioClip HittingSpinAudioClip;
    [SerializeField] private AudioClip HittingKeyAudioClip;
    [SerializeField] private AudioClip pickSuccessAudioClip;
    [SerializeField] private AudioClip pickFailAudioClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayBackGroundMusic();
        PlaySpinningSound();
    }
    public void PlayBackGroundMusic()
    {
        backGroundAudioSource.clip = backGroundAudioClip;
        backGroundAudioSource.Play();
    }
    public void PlaySpinningSound()
    {
        spinningAudioSource.clip = SpiningAudioClip;
        spinningAudioSource.Play();
    }
    public void PlayUnlockSound()
    {
        effectAudioSource.PlayOneShot(pickSuccessAudioClip);
    }
    public void HittingSpinSound()
    {
        effectAudioSource.PlayOneShot(HittingSpinAudioClip);
    }
    public void HittingKeySound()
    {
        effectAudioSource.PlayOneShot(HittingKeyAudioClip);
    }
    public void ShootingKeySound()
    {
        effectAudioSource.PlayOneShot(ShootingKeyAudioClip);
    }
    public void PlayFailSound()
    {
        effectAudioSource.PlayOneShot(pickFailAudioClip);
    }
    public void StopAllSounds()
    {
        backGroundAudioSource.Stop();
        spinningAudioSource.Stop();
        effectAudioSource.Stop();
    }
}
