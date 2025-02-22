using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public enum Difficulty
{
    Level1,
    Level2, 
    Level3
}

public static class SimonDifficulties
{
    
}

public class SimonSaysScript : MonoBehaviour
{
    [SerializeField] private int passwordIndex;
    [SerializeField] public string[] availableInputs;
    [SerializeField] private int passwordLength;
    [SerializeField] string[] passwordArray;
    [SerializeField] private SpriteRenderer _incorrectSprite;
    [SerializeField] private SpriteRenderer _successSprite;
    [SerializeField] private SpriteRenderer _upSprite;
    [SerializeField] private SpriteRenderer _downSprite;
    [SerializeField] private TextMeshProUGUI _timeText;
    private AudioSource clickSound;

    [SerializeField] private float timeGap;
    [HideInInspector] public bool allowInput = true;
    [SerializeField] private bool isSuccess;

    public Difficulty difficulty;

    // Coroutines
    private Coroutine FlashCoroutine;
    private Coroutine ShowDirCoroutine;
    private Coroutine ShowResultCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickSound = GetComponent<AudioSource>();
        _timeText = GetComponentInChildren<TextMeshProUGUI>();

        //_timeText.text = $"{timeRemain}";
        //_incorrectSprite.enabled = false;
        //_successSprite.enabled = false;

        ExitGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExitGame();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartGame();
        }

        if (!allowInput)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreatePassword();
            StartFlash();
        }
    }

    string GetPasswordIndex()
    {
        return passwordArray[passwordIndex++];
    }

    public void OnButtonPressed(string btnName)
    {
        print($"The {btnName.ToUpper()} button was pressed.");
        clickSound.Stop();
        clickSound.Play();
        // Return if the password isn't setup
        if (!passwordArray.Any())
        {
            return;
        }

        if (btnName == GetPasswordIndex())
        {

        }
        else
        {
            print($"Incorrect");
            ShowResultSprite(false);
            passwordIndex = 0;
        }

        // Correctly inputed the password
        if (passwordIndex == passwordArray.Length)
        {
            ShowResultSprite(true, true);
            passwordIndex = 0;
            allowInput = false;
            print("Unlocked");
            if (CountDownCoroutine != null)
            {
                StopCoroutine(CountDownCoroutine);
            }
        }
    }

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


    public void StartFlash()
    {
        if (FlashCoroutine != null)
        {
            StopCoroutine(FlashCoroutine);
        }

        FlashCoroutine = StartCoroutine(StartFlashCoroutine());

        IEnumerator StartFlashCoroutine()
        {
            allowInput = false;
            yield return new WaitForSeconds(2f);

            foreach (var pass in passwordArray)
            {
                var button = transform.Find(pass);
                clickSound.Stop();
                clickSound.Play();
                if (button != null)
                {
                    button?.GetComponent<SimonButtonScript>().Blink(timeGap * 0.7f);
                }
                else
                {
                    ShowDir(pass, timeGap * 0.7f);
                }
                yield return new WaitForSeconds(timeGap);
            }

            // 
            allowInput = true;
            CountDownTimer();
        }
    }

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
            _successSprite.enabled = result && persist? true: false;
            _incorrectSprite.enabled = !result && persist ? true : false;
        }
    }

    [SerializeField] private float timeRemain;
    private Coroutine CountDownCoroutine;

    private void CountDownTimer()
    {
        if (CountDownCoroutine != null)
        {
            StopCoroutine(CountDownCoroutine);
        }

        CountDownCoroutine = StartCoroutine(StartCountDown());

        IEnumerator StartCountDown()
        {
            var time = timeRemain;

            while (time > 0f)
            {
                _timeText.text = $"{Mathf.Ceil(time)}";
                time -= Time.deltaTime;

                yield return new WaitForSeconds(Time.deltaTime); 
            }

            _timeText.text = "0";
            if (time <= 0f && !isSuccess)
            {
                ShowResultSprite(false, true);
                allowInput = false;
            }

        }
    }

    /// <summary>
    /// Function to start the mini-game
    /// ** Will be called by a parent Canvas class **
    /// </summary>
    public void StartGame()
    {
        StopAllCoroutines();
        allowInput = true;
        transform.GetComponent<Renderer>().enabled = true;
        _timeText.enabled = true;
        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in sprites)
        {
            item.enabled = true;
        }

        _timeText.text = $"{timeRemain}";
         CreatePassword();
        StartFlash();
    }

    /// <summary>
    /// Function to stop and exit the mini-game
    /// ** Will be called by a parent Canvas class **
    /// </summary>
    public void ExitGame()
    {
        StopAllCoroutines();
        allowInput = false;
        transform.GetComponent<Renderer>().enabled = false;
        _timeText.enabled = false;
        var sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var item in sprites)
        {
            item.enabled = false;
        }
    }
}
