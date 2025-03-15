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

[SelectionBase]
public class SimonSaysScript : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _mainModel;
    [SerializeField] private SpriteRenderer _incorrectSprite;
    [SerializeField] private SpriteRenderer _successSprite;
    [SerializeField] private SpriteRenderer _upSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    [SerializeField] private TextMeshPro _timeText;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private Transform _ledResultCounter;

    [Header("Game Info")]
    [SerializeField] private LayerMask inputLayerMask;
    [SerializeField] private int passwordIndex;
    [SerializeField] public string[] availableInputs;
    [SerializeField] string[] passwordArray;

    [Header("Game settings")]
    [SerializeField] private int passwordLength;
    [SerializeField] private float timeGap; // Time gap between each password indication
    [SerializeField] private float countDownTime; // Time to solve minigame
    [HideInInspector] public bool allowInput = true;
    [SerializeField] private bool isSuccess;
    [SerializeField] private float failPenalty; // Noise penalty for failing game
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

    private void Awake()
    {
        GameManagerSingleton.SimonGame = this;

        _ledResultCounter = transform.Find("LedResultCounter");
    }

    void Start()
    {
        ExitGame();

        Camera.main.eventMask = inputLayerMask;
    }

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
    /// Handles the password input
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
            Debug.LogWarning($"{nameof(passwordArray)} hasn't been generated");
            return;
        }

        if (btnName == passwordArray[passwordIndex])
        {
            // The input is correct
            ShowResult(true);
            passwordIndex++;

            // Successfully inputed the password
            if (passwordIndex == passwordArray.Length)
            {
                ShowResult(true, true);
                print("Unlocked");
                ExitGame(1f);
            }
        }
        else
        {
            // The input is wrong
            currentAttempts++;
            passwordIndex = 0;
            ShowResult(false);

            // penalize when out of attemps
            if (currentAttempts >= maxAttempts)
            {
                GameManagerSingleton.NoiseController?.AddNoise(failPenalty);
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
            yield return new WaitForSeconds(1.75f);

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
    private void ShowResult(bool result, bool persist = false)
    {
        var loopCount = result ? 3 : currentAttempts;
        for (int i = 0; i < 3; i++)
        {
            var ledResult = _ledResultCounter.GetChild(i).GetComponent<ResultLed>();

            if (i < loopCount)
            {
                ledResult.SetResult(result);
            }
            else
            {
                ledResult.TurnOff();
            }
        }

        if (ShowResultCoroutine != null)
        {
            StopCoroutine(ShowResultCoroutine);
        }

        ShowResultCoroutine = StartCoroutine(StartShowResult());

        IEnumerator StartShowResult()
        {
            var blinkCounter = 0;

            while (blinkCounter < 2)
            {
                _successSprite.enabled = result;
                _incorrectSprite.enabled = !result;
                yield return new WaitForSeconds(.3f);

                _successSprite.enabled = false;
                _incorrectSprite.enabled = false;
                blinkCounter++;
                yield return new WaitForSeconds(.3f);
            }

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
                yield return new WaitForSeconds(Time.deltaTime);

                time -= Time.deltaTime;
                _timeText.text = $"{Mathf.Ceil(time)}";
            }

            // If is not successful after time runs out Exit (& penalize)
            yield return new WaitUntil(() => time <= 0f && !isSuccess);
            _timeText.text = "0";
            ShowResult(false, true);
            allowInput = false;
            GameManagerSingleton.NoiseController?.AddNoise(failPenalty);

            // Wait 1.8 seconds then exit
            ExitGame(1.8f);
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


        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in sprites)
        {
            item.enabled = true;
        }

        foreach (var ledResult in _ledResultCounter.GetComponentsInChildren<ResultLed>())
        {
            ledResult.TurnOff();
        }

        _timeText.text = $"{countDownTime}";
        _timeText.enabled = true;

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
            allowInput = false;
            yield return new WaitForSeconds(delay);

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
