using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockPickRotation : MonoBehaviour
{
    private LockPickAudioController audioController;

    private void Awake()
    {
        audioController = FindAnyObjectByType<LockPickAudioController>();
    }
    //handle the stick
    public Transform lockPick; 
    public float rotationSpeed = 5f; 

    private bool isRotating = false;
    private Vector3 lastMousePosition;


    //handle the spin lock
    public Transform innerLock; 

    public float maxAngle = 140f;
    public float lockSpeed = 10f;
    [Min(1)]
    [Range(1, 25)]
    public float lockRange = 10f;

    private float eulerAngle;
    private float unlockAngle;
    private Vector2 unlockRange;

    private float keyPressTime = 0;

    //Handle stress bar
    private float stressLevel = 0f;
    private float stressThreshold = 100f;
    [SerializeField] private float stressIncreaseRate = 50f;
    [SerializeField] private float stressDecreaseRate = 20f;

    // add stress bar
    public Slider StressBar;

    // add countdown timer
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float remainingTime;


    void Start()
    {
        StressBar.value = 0;
        NewLock();
    }

    void Update()
    {
        HandlePickRotation();
        HandleLockRotation();
        CheckPickStress();
        CountDownTime();
    }

    void NewLock()
    {
        unlockAngle = UnityEngine.Random.Range(-maxAngle + lockRange, maxAngle - lockRange);
        unlockRange = new Vector2(unlockAngle - lockRange, unlockAngle + lockRange);
    }
    void HandlePickRotation()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            isRotating = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && isRotating)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationAmount = delta.x * rotationSpeed * Time.deltaTime; // Rotate based on X-axis movement
  
            eulerAngle = Mathf.Clamp(eulerAngle + rotationAmount, -maxAngle, maxAngle);

            lockPick.rotation = Quaternion.Euler(0, 0, eulerAngle);
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Release mouse button
        {
            isRotating = false;
        }
    }

    void HandleLockRotation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            keyPressTime = 1;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            keyPressTime = 0;
            StartCoroutine(ResetInnerLock()); // Smoothly return to original position
        }

        keyPressTime = Mathf.Clamp(keyPressTime, 0, 1);

        float percentage = Mathf.Round(100 - Mathf.Abs(((eulerAngle - unlockAngle) / maxAngle) * 100));
        float lockRotation = ((percentage / 100) * maxAngle) * keyPressTime;

        if (keyPressTime > 0)
        {
            // Smoothly interpolate rotation using Quaternion to avoid incorrect negative values
            Quaternion targetRotation = Quaternion.Euler(0, 0, lockRotation);
            innerLock.rotation = Quaternion.Lerp(innerLock.rotation, targetRotation, Time.deltaTime * lockSpeed);
            float angleDifference = Mathf.Abs(eulerAngle - unlockAngle);

            //Play sound effect
            if (angleDifference > lockRange * 1.5f) // Far from unlock position
            {
                audioController.PlayPickLong2Sound();
            }
            else if (angleDifference > lockRange * 1f) // Moderately close
            {
                audioController.PlayPickLong1Sound();
            }
            else if (angleDifference > lockRange * 0.5f) // Moderately close
            {
                audioController.PlayPickShortSound();
            }

            if (Mathf.Abs(innerLock.eulerAngles.z - lockRotation) < 1f) // Ensure we check close enough
            {
                if (eulerAngle < unlockRange.y && eulerAngle > unlockRange.x)
                {
                    Debug.Log("Unlocked");
                    audioController.PlayUnlockSound();
                    lockPick.gameObject.SetActive(false); // Ẩn pick khi bị gãy

                    FindObjectOfType<InteractableController>().isSuccess = true;
                    FindObjectOfType<InteractableController>().CloseLockPickGame();
                }
                else
                {
                    float randomRotation = UnityEngine.Random.insideUnitCircle.x;
                    lockPick.eulerAngles += new Vector3(0, 0, UnityEngine.Random.Range(-randomRotation, randomRotation));
                }
            }
        }
    }

    void CheckPickStress()
    {
        if (keyPressTime > 0)
        {
            // Nếu pick lệch quá xa khỏi vùng mở khóa -> tăng độ căng
            if (eulerAngle < unlockRange.x - lockRange || eulerAngle > unlockRange.y + lockRange)
            {
                stressLevel += stressIncreaseRate * Time.deltaTime;
                StressBar.value = stressLevel;
            }
            else
            {
                // Nếu đang ở gần đúng góc, giảm căng
                stressLevel -= stressDecreaseRate * Time.deltaTime;
                StressBar.value = stressLevel;
            }
        }
        else
        {
            // Nếu không nhấn, từ từ giảm căng
            stressLevel -= stressDecreaseRate * Time.deltaTime * 0.5f;
            StressBar.value = stressLevel;
        }

        stressLevel = Mathf.Clamp(stressLevel, 0, stressThreshold);

        // Kiểm tra nếu pick bị gãy
        if (stressLevel >= stressThreshold)
        {
            StressBar.value = stressLevel;
            Debug.Log("Game Over - You fail");
            GameOver();
        }
    }

    void GameOver()
    {
        audioController.PlayFailSound();
        StartCoroutine(BreakPickAnimation());
    }

    IEnumerator BreakPickAnimation()
    {
        float duration = 0.3f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            lockPick.localScale *= 0.9f; // Làm pick nhỏ dần như bị gãy
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lockPick.gameObject.SetActive(false); // Ẩn pick khi bị gãy
        FindObjectOfType<InteractableController>().isSuccess = false;
        FindObjectOfType<InteractableController>().CloseLockPickGame();

    }

    IEnumerator ResetInnerLock()
    {
        float startRotation = innerLock.eulerAngles.z;
        float elapsedTime = 0;
        float duration = 0.5f;

        while (elapsedTime < duration)
        {
            float lerpedRotation = Mathf.LerpAngle(startRotation, 0, elapsedTime / duration);
            innerLock.rotation = Quaternion.Euler(0, 0, lerpedRotation);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        innerLock.rotation = Quaternion.Euler(0, 0, 0);
    }

    void CountDownTime()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0.0f)
        {
            Debug.Log("Game Over");
            GameOver();
            return;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}

