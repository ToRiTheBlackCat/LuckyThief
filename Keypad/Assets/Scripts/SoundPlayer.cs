using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource soundPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound()
    {
        soundPlayer.Play();
    }
}
