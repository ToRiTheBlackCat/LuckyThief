using Assets.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SafeDial : MiniGameBase
{
    private const float FULL_ROTATION = 100f;
    private const float DEGREE_PER_NUMBER = 3.6f;
    private const float ANGLE_TOLERANCE = 7.2f; // 2 numbers tolerance

    public int[] combination { get; set; } = new int[3];
    private int combinationIndex = 0;
    private bool isUnlocked = false;
    private bool waitingForReverse = false;
    private bool isDialLocked = false;
    private AudioSource audioSource;

    public AudioClip SafeClickFast;
    public AudioClip SafeClick;
    public TMP_Text SafePassDisplay;
    public TMP_Text SafeUnlockedText;
    private InteractableScript _attachedInteractable;
    private Transform DialKnob;

    public float RotationSpeed { get; set; } = 25f; // Further reduced rotation speed
    private float currentDialAngle = 0f;
    private float targetDialAngle = 0f;

    void Start()
    {

        SafePassDisplay = transform.Find("PaperNote").Find("SafePassDisplay").GetComponent<TMP_Text>();
        DialKnob = transform.Find("SafeDialInner_0").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();

        InitializeCombination();

        SafeUnlockedText.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ExitGame();
            return;
        }
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
        currentDialAngle = DialKnob.localEulerAngles.z;
        CheckCombination(currentDialAngle, rotation);

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartGame(null);
            return;
        }
    }

    private void InitializeCombination()
    {
        combination = new int[] { Random.Range(1, 99), Random.Range(0, 99), Random.Range(0, 99) };
        SafePassDisplay.text = $"{combination[0]} {combination[1]} {combination[2]}";
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
        DialKnob.localRotation = Quaternion.Euler(0, 0, -currentDialAngle * DEGREE_PER_NUMBER);


        PlayTurningSound();

        // Debug: Display the current dial angle and number
        int currentDialNumber = GetDialNumberFromAngle(currentDialAngle);
        Debug.Log($"Current Dial Angle: {currentDialAngle}, Current Dial Number: {currentDialNumber}");
    }

    private void CheckCombination(float dialAngle, float rotationDirection)
    {
        if (combinationIndex > 2) return;

        int currentDialNumber = GetDialNumberFromAngle(dialAngle);
        int targetNumber = combination[combinationIndex];

        // Debug: Display the target number and current dial number
        Debug.Log($"Target Number: {targetNumber}, Current Dial Number: {currentDialNumber}");

        if (Mathf.Abs(currentDialNumber - targetNumber) < ANGLE_TOLERANCE / DEGREE_PER_NUMBER)
        {
            if (combinationIndex < 2 || IsCorrectRotationDirection(rotationDirection))
            {
                combinationIndex++;
                StopTurningSound();
                PlayClickSound();
                isDialLocked = true;
                waitingForReverse = !waitingForReverse;

                if (combinationIndex > 2)
                {
                    isUnlocked = true;
                    Debug.Log("Safe Unlocked!");
                    if (SafeUnlockedText != null)
                    {
                        SafeUnlockedText.text = "Safe Unlocked!";
                        SafeUnlockedText.gameObject.SetActive(true);

                        _attachedInteractable.OnAttachedMinigameSuccess();
                        _attachedInteractable = null;
                    }
                    enabled = false;
                    ExitGame();
                    //StartCoroutine(ReturnToGameScene());
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

    public override void StartGame(InteractableScript attachedInteractable = null)
    {
        GameManagerSingleton.Player.SpeedMult = 0f;

        if (attachedInteractable != null)
        {
            _attachedInteractable = attachedInteractable;
        }

        gameObject.SetActive(true);
        SafeUnlockedText.gameObject.SetActive(false);
    }

    public override void ExitGame(float delay = 0)
    {
        gameObject.SetActive(false);
        GameManagerSingleton.Player.SpeedMult = 1f;
    }

    //private IEnumerator ReturnToGameScene()
    //{
    //    yield return new WaitForSeconds(1f);
    //    // Return to the main scene
    //    SceneManager.UnloadSceneAsync("SafeDial");
    //    Theif player = FindFirstObjectByType<Theif>();
    //    if (player != null)
    //    {
    //        player.enabled = true;
    //    }
    //}


}
