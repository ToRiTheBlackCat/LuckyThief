using System;
using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class audioManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private AudioSource defaultAudioSource;
        [SerializeField] private AudioSource effectAudioSource;
        [SerializeField] private AudioClip alertClip;
        [SerializeField] private AudioClip robotClip;
        [SerializeField] private AudioClip dogClip;
        [SerializeField] private AudioClip takeDamageClip;
        [SerializeField] private AudioClip dieClip;

        public void PlayAlert()
        {
            effectAudioSource.clip = alertClip;
            effectAudioSource.Play();
        }
        public void StopAlert()
        {
            effectAudioSource.Stop();
        }
        public void PlayBackgroundMusic()
        {
            //defaultAudioSource.clip = backgroundClip;
            defaultAudioSource.Play();
        }
        public void StopMusic()
        {
            defaultAudioSource.Stop();
            effectAudioSource.Stop();
        }
        public void Playrobot()
        {
            effectAudioSource.PlayOneShot(robotClip);
        }
        public void StopEffect()
        {
            effectAudioSource.Stop();           
        }
        public void PlayDog()
        {
            effectAudioSource.PlayOneShot(dogClip);
        }
        public void PlayTakeDamagePlayer()
        {
            effectAudioSource.PlayOneShot(takeDamageClip);
        }
        public void PlayDead()
        {
            effectAudioSource.PlayOneShot(dieClip);
        }
    }
}