using Assets.Scripts;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum Difficulty
{
    Level1,
    Level2,
    Level3
}

public class SimonSaysScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mainModel;
    [SerializeField] private SpriteRenderer _incorrectSprite;
    [SerializeField] private SpriteRenderer _successSprite;
    [SerializeField] private SpriteRenderer _upSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    [SerializeField] private TextMeshPro _timeText;
    [SerializeField] private AudioSource clickSound;

    [Header("Game Info")]
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private int passwordIndex;
    [SerializeField] public string[] availableInputs;
    [SerializeField] string[] passwordArray;

    [Header("Game settings")]
    [SerializeField] private int passwordLength;
    [SerializeField] private float timeGap;
    [SerializeField] private float countDownTime;
    [HideInInspector] public bool allowInput = true;
    [SerializeField] private bool isSuccess;
    [SerializeField] private float failPenalty;
    [SerializeField] private int currentAttempts;
    [SerializeField] private int maxAttempts;
    public Difficulty difficulty;

    // Coroutines
    private Coroutine FlashCoroutine;
    private Coroutine ShowDirCoroutine;
    private Coroutine ShowResultCoroutine;
    private Coroutine CountDownCoroutine;
    private Coroutine ExitCoroutine;

    // Events
    public UnityEvent<float> onFail;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ExitGame();
        GameManagerSingleton.SimonGame = this;
        Camera.main.eventMask = inputLayerMask;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExitGame();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            StartGame();
        }
    }

    /// <summary>
    /// Handles the input
    /// </summary>
    /// <param name="btnName">Name of the input</param>
    public void OnButtonPressed(string btnName)
    {
        print($"The {btnName.ToUpper()} button was pressed.");
        clickSound.Stop();
        clickSound.Play();

        // Return if the password hasn't been generated setup
        if (!passwordArray.Any())
        {
            Debug.LogError($"{nameof(passwordArray)} hasn't been generated");
            return;
        }

        if (btnName == passwordArray[passwordIndex])
        {
            // The input is correct
            ShowResultSprite(true);
            passwordIndex++;

            // Successfully inputed the password
            if (passwordIndex == passwordArray.Length)
            {
                ShowResultSprite(true, true);
                allowInput = false;
                print("Unlocked");
                ExitGame(1f);
            }
        }
        else
        {
            // The input is wrong
            ShowResultSprite(false);
            passwordIndex = 0;
            currentAttempts++;

            // penalize when out of attemps
            if (currentAttempts >= maxAttempts)
            {
                GameManagerSingleton.NoiseController.AddNoise(failPenalty);
                allowInput = false;
                ExitGame(1f);
            }
        }
    }

    /// <summary>
    /// Auto generate password.
    /// </summary>
    public void CreatePassword()
    {
        passwordIndex = 0;
        passwordArray = new string[0];

        _incorrectSprite.enabled = false;
        _successSprite.enabled = false;
        _downSprite.enabled = false;
        _upSprite.enabled = false;

        while (passwordArray.Count() < passwordLength)
        {
            var randi = Random.Range(0, availableInputs.Count());
            var randomInput = availableInputs.ElementAt(randi);
            passwordArray = passwordArray.Append(randomInput).ToArray();
        }
    }

    /// <summary>
    /// Start showing password sequence based on
    /// generated password array.
    /// </summary>
    private void StartFlash()
    {
        if (FlashCoroutine != null)
        {
            StopCoroutine(FlashCoroutine);
        }

        FlashCoroutine = StartCoroutine(StartFlashCoroutine());

        IEnumerator StartFlashCoroutine()
        {
            // Stop user input
            allowInput = false;
            yield return new WaitForSeconds(1.25f);

            foreach (var pass in passwordArray)
            {
                var button = transform.Find(pass);
                clickSound.Stop();
                clickSound.Play();
                if (button != null)
                {
                    // Blink if is button input
                    button?.GetComponent<SimonButtonScript>().Blink(timeGap * 0.7f);
                }
                else
                {
                    // Show dir if lever input
                    ShowDir(pass, timeGap * 0.7f);
                }
                yield return new WaitForSeconds(timeGap);
            }

            // allow user input
            allowInput = true;
            CountDownTimer();
        }
    }

    /// <summary>
    /// Show arrow for lever input
    /// </summary>
    /// <param name="leverInput"></param>
    /// <param name="duration"></param>
    private void ShowDir(string leverInput, float duration)
    {
        if (ShowDirCoroutine != null)
        {
            StopCoroutine(ShowDirCoroutine);
        }

        ShowDirCoroutine = StartCoroutine(StartShowDir());

        IEnumerator StartShowDir()
        {
            var isUp = leverInput.Contains("Up");

            _upSprite.enabled = isUp;
            _downSprite.enabled = !isUp;
            yield return new WaitForSeconds(duration);
            _upSprite.enabled = false;
            _downSprite.enabled = false;
        }
    }

    /// <summary>
    /// Show the correct/wrong result sprite
    /// </summary>
    /// <param name="result">False: Wrong sprite. True: Correct sprite</param>
    /// <param name="persist">If the sprite stay enabled after flashing</param>
    private void ShowResultSprite(bool result, bool persist = false)
    {
        if (ShowResultCoroutine != null)
        {
            StopCoroutine(ShowResultCoroutine);
        }

        ShowResultCoroutine = StartCoroutine(StartShowResult());

        IEnumerator StartShowResult()
        {
            _successSprite.enabled = result;
            _incorrectSprite.enabled = !result;
            yield return new WaitForSeconds(.3f);
            _successSprite.enabled = false;
            _incorrectSprite.enabled = false;
            yield return new WaitForSeconds(.3f);
            _successSprite.enabled = result;
            _incorrectSprite.enabled = !result;
            yield return new WaitForSeconds(.3f);
            _successSprite.enabled = result && persist ? true : false;
            _incorrectSprite.enabled = !result && persist ? true : false;
        }
    }

    /// <summary>
    /// Start the count-down timer
    /// </summary>
    private void CountDownTimer()
    {
        if (CountDownCoroutine != null)
        {
            StopCoroutine(CountDownCoroutine);
        }

        CountDownCoroutine = StartCoroutine(StartCountDown());

        IEnumerator StartCountDown()
        {
            var time = countDownTime;

            while (time > 0f)
            {
                _timeText.text = $"{Mathf.Ceil(time)}";
                time -= Time.deltaTime;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            // If is not successful after time runs out Exit (& penalize)
            yield return new WaitUntil(() => time <= 0f && !isSuccess);
            _timeText.text = "0";
            ShowResultSprite(false, true);
            allowInput = false;
            GameManagerSingleton.NoiseController?.AddNoise(failPenalty);

            // Wait 1.8 seconds then exit
            yield return new WaitForSeconds(1.8f);
            ExitGame();
        }
    }

    /// <summary>
    /// Function to start the mini-game
    /// ** Will be called by a parent Canvas class **
    /// </summary>
    public void StartGame()
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        currentAttempts = 0;
        allowInput = true;

        _timeText.enabled = true;
        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in sprites)
        {
            item.enabled = true;
        }

        _timeText.text = $"{countDownTime}";
        CreatePassword();
        StartFlash();
    }

    /// <summary>
    /// Function to stop and exit the mini-game
    /// ** Will be called by a parent Canvas class **
    /// </summary>
    public void ExitGame(float delay = 0f)
    {
        if (ExitCoroutine != null)
        {
            StopCoroutine(ExitCoroutine);
        }
        ExitCoroutine = StartCoroutine(StartExitGame());

        IEnumerator StartExitGame()
        {
            yield return new WaitForSeconds(delay);

            allowInput = false;
            _timeText.enabled = false;
            var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (var item in sprites)
            {
                item.enabled = false;
            }
            gameObject.SetActive(false);
        }
    }
}
