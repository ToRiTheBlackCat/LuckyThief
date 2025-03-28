using System;
using UnityEngine;
namespace LuckyThief.ThangScripts
{
    public class audioManager : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private AudioSource background;
        [SerializeField] private AudioClip backgroundClip;
        void Start()
        {
            PlayBackgroundMusic();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void PlayBackgroundMusic()
        {
            background.clip = backgroundClip;
            background.Play();
        }
    }
}