using TMPro;
using UnityEngine;

public class SafeDial : MonoBehaviour
{
    private const float FULL_ROTATION = 100f;
    private const float DEGREE_PER_NUMBER = 3.6f;
    private const float ANGLE_TOLERANCE = 7.2f; // 2 numbers tolerance

    public int[] Combination { get; set; } = new int[3];
    private int combinationIndex = 0;
    private bool isUnlocked = false;
    private bool waitingForReverse = false;
    private bool isDialLocked = false;
    private AudioSource audioSource;

    public AudioClip SafeClickFast;
    public AudioClip SafeClick;
    public TMP_InputField SafePassDisplay;
    public TMP_Text SafeUnlockedText;

    public float RotationSpeed { get; set; } = 25f; // Further reduced rotation speed
    private float currentDialAngle = 0f;
    private float targetDialAngle = 0f;

    void Start()
    {
        InitializeSafePassDisplay();
        InitializeCombination();
        audioSource = GetComponent<AudioSource>();
        DisplayCombination();
        if (SafeUnlockedText != null)
        {
            SafeUnlockedText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isUnlocked) return;

        float rotation = GetRotationInput();
        if (rotation == 0)
        {
            StopTurningSound();
            return;
        }

        if (IsCorrectRotationDirection(rotation))
        {
            isDialLocked = false;
        }

        if (isDialLocked) return;

        ApplyRotation(rotation);
        currentDialAngle = transform.localEulerAngles.z;
        CheckCombination(currentDialAngle, rotation);
    }

    private void InitializeSafePassDisplay()
    {
        if (SafePassDisplay == null)
        {
            SafePassDisplay = GameObject.Find("SafePassDisplay").GetComponent<TMP_InputField>();
        }
    }

    private void InitializeCombination()
    {
        Combination = new int[] { Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99) };
    }

    private void DisplayCombination()
    {
        SafePassDisplay.text = $"{Combination[0]} {Combination[1]} {Combination[2]}";
    }

    private float GetRotationInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            return RotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            return -RotationSpeed * Time.deltaTime;
        }
        return 0f;
    }

    private bool IsCorrectRotationDirection(float rotation)
    {
        return (waitingForReverse && rotation < 0) || (!waitingForReverse && rotation > 0);
    }

    private void ApplyRotation(float rotation)
    {
        Debug.Log($"Rotation Input: {rotation}");

        targetDialAngle += rotation * (FULL_ROTATION / 360f);
        targetDialAngle = (targetDialAngle + FULL_ROTATION) % FULL_ROTATION;
        currentDialAngle = targetDialAngle; // remove Lerp for testing
        transform.localRotation = Quaternion.Euler(0, 0, -currentDialAngle * DEGREE_PER_NUMBER);


        PlayTurningSound();

        // Debug: Display the current dial angle and number
        int currentDialNumber = GetDialNumberFromAngle(currentDialAngle);
        Debug.Log($"Current Dial Angle: {currentDialAngle}, Current Dial Number: {currentDialNumber}");
    }

    private void CheckCombination(float dialAngle, float rotationDirection)
    {
        if (combinationIndex >= Combination.Length) return;

        int currentDialNumber = GetDialNumberFromAngle(dialAngle);
        int targetNumber = Combination[combinationIndex];

        // Debug: Display the target number and current dial number
        Debug.Log($"Target Number: {targetNumber}, Current Dial Number: {currentDialNumber}");

        if (Mathf.Abs(currentDialNumber - targetNumber) < ANGLE_TOLERANCE / DEGREE_PER_NUMBER)
        {
            if (combinationIndex == 0 || IsCorrectRotationDirection(rotationDirection))
            {
                StopTurningSound();
                PlayClickSound();
                isDialLocked = true;

                combinationIndex++;
                waitingForReverse = !waitingForReverse;

                if (combinationIndex >= 2)
                {
                    isUnlocked = true;
                    Debug.Log("Safe Unlocked!");
                    if (SafeUnlockedText != null)
                    {
                        SafeUnlockedText.text = "Safe Unlocked!";
                        SafeUnlockedText.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    private int GetDialNumberFromAngle(float angle)
    {
        // Normalize the angle to be within 0 to 360 degrees
        angle = (angle + 360) % 360;

        // Calculate the dial number based on the normalized angle
        return Mathf.RoundToInt(angle / DEGREE_PER_NUMBER);
    }

    private void PlayTurningSound()
    {
        if (audioSource != null && !audioSource.isPlaying && SafeClickFast != null)
        {
            audioSource.loop = true;
            audioSource.clip = SafeClickFast;
            audioSource.Play();
        }
    }

    private void StopTurningSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlayClickSound()
    {
        if (audioSource != null && SafeClick != null)
        {
            audioSource.PlayOneShot(SafeClick);
        }
    }

    private int[] GetRandomPass()
    {
        int[] pass = { Random.Range(0, 99), Random.Range(0, 99), Random.Range(0, 99) };

        return pass;
    }
}
