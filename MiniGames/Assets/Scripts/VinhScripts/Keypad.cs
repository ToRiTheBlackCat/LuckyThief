using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Audio;

namespace LuckyThief.VinhScripts
{
    public class Keypad : MonoBehaviour
    {
        public string currPassword;
        public TMP_InputField displayPassword;
        public int charLimit = 4;
        public TMP_InputField charHolder;
        public GameObject button1;
        public GameObject button2;
        public GameObject button3;
        public GameObject button4;
        public GameObject button5;
        public GameObject button6;
        public GameObject button7;
        public GameObject button8;
        public GameObject button9;
        public GameObject buttonClear;
        public GameObject button0;
        public GameObject buttonEnter;
        public AudioClip jackpotAudioClip;
        public AudioClip sirenAudioClip;
        public AudioClip coinFailAudioClip;
        public AudioClip buttonAudioClip;
        public AudioSource audioSource;
        bool actionDisabled = false;
        public bool isSolved = false;
        public bool isAlarm = false;

        public bool checkCharLimit(string currInput)
        {
            if (currInput.Length >= charLimit)
            {
                return false;
            }
            else return true;
        }
        public void b1()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "1";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b2()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "2";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b3()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "3";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b4()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "4";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b5()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "5";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b6()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "6";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b7()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "7";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b8()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "8";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b9()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "9";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void b0()
        {
            if (checkCharLimit(charHolder.text))
            {
                charHolder.text += "0";
            }
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public void bClear()
        {
            charHolder.text = null;
            audioSource.PlayOneShot(buttonAudioClip, 1);
        }
        public async void bEnter()
        {
            if (!actionDisabled)
            {
                audioSource.PlayOneShot(buttonAudioClip, 1);
                if (charHolder.text == currPassword)
                {
                    isSolved = true;
                    charHolder.text = "<color=#00FF00>Correct</color>";
                    audioSource.PlayOneShot(jackpotAudioClip, 1);
                    actionDisabled = true;
                    await Task.Delay(3000);
                    actionDisabled = false;
                    bClear();
                }
                else
                {
                    isAlarm = true;
                    charHolder.text = "<color=#FF0000>Wrong</color>";
                    audioSource.PlayOneShot(sirenAudioClip, 1);
                    actionDisabled = true;
                    await Task.Delay(3000);
                    actionDisabled = false;
                    bClear();
                }
            }
        }
        //TODO: Max password attempts before alarm and cant be unlock for 5m
        public string randomPassword()
        {
            int _min = 0;
            int _max = 9999;
            System.Random _rdm = new System.Random();
            return String.Format("{0:0000}", _rdm.Next(_min, _max));
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Start()
        {
            currPassword = randomPassword();
            displayPassword.text = currPassword;
            isSolved = false;
            isAlarm = false;
        }

        // Update is called once per frame
        void Update()
        {

        }
    } 
}
