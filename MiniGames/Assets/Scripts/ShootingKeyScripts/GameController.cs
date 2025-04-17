using Assets.Scripts;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MiniGameBase
{
    //SPIN
    [SerializeField] Transform fruitSpin;
    public float rotationSpeed = 1f;
    private bool isRight = true;

    public int minSpeed;
    public int maxSpeed;


    [SerializeField] private GameObject keyPrefab;
    public Transform keySpawnPoint;

    [SerializeField] TextMeshPro timer;
    [SerializeField] float remainingTime;
    
    private ShootingKeyAudioController audioController;

    public int keyNumber;
    private int initialKeyNumber;
    public Transform spin;
    private bool isGameOver = false; // Add this flag

    //private InteractableController interactableController;
    [SerializeField] private float failPenalty;
    private void Awake()
    {
        audioController = FindAnyObjectByType<ShootingKeyAudioController>();
       
        //interactableController = FindAnyObjectByType<InteractableController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //initialKeyNumber = keyNumber;
        // Khởi động lại các coroutine
        StartCoroutine(CountDownTime());
        StartCoroutine(CheckDirectionChange());
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }
    void Spin()
    {
        fruitSpin.transform.Rotate((isRight ? 1 : -1) * Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    IEnumerator CheckDirectionChange()
    {
        while (true)
        {

            yield return new WaitForSeconds(5f);
            rotationSpeed = RandomRotationSpeed();
            isRight = RandomLeftRight();
        }
    }
    float RandomRotationSpeed()
    {
        var r = Random.Range(minSpeed, maxSpeed);
        if (r > 150)
            rotationSpeed = r;
        return rotationSpeed;
    }

    bool RandomLeftRight()
    {
        return Random.Range(0, 1) > 0.5f;
    }

    public void SpawnNewKey()
    {
        if (keyNumber != 0)
        {
            Instantiate(keyPrefab, keySpawnPoint.position, keySpawnPoint.rotation);
            keyNumber--;
        }

    }

    public void GameWin()
    {
        Debug.Log("You Win");
        audioController.PlayUnlockSound();
        spin.gameObject.SetActive(false);
        StopAllCoroutines();

        //interactableController.isSuccess = true;
        //interactableController.CloseGame();
        _attachedInteractable?.OnAttachedMinigameSuccess();
        _attachedInteractable = null;
        ExitGame(1f);
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls

        isGameOver = true;
        Debug.Log("Game Over");
        // Stop all sounds
        audioController.StopAllSounds();

        audioController.PlayFailSound();
        StartCoroutine(DisapearSpinAnimation());
        

    }

    IEnumerator DisapearSpinAnimation()
    {
        float duration = 0.3f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            spin.localScale *= 0.9f;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        //spin.gameObject.SetActive(false);

        StopAllCoroutines();

        GameManagerSingleton.NoiseController.AddNoise(failPenalty);
        ExitGame(1f);
        //interactableController.isSuccess = false;
        //interactableController.CloseGame();

    }
    IEnumerator CountDownTime()
    {
        if (isGameOver) yield return null;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0.0f)
        {
            GameOver();
            yield return null;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void ResetGame()
    {
        StopAllCoroutines(); // Dừng tất cả coroutine
        isGameOver = false; // Đặt lại trạng thái game

        // Đặt lại thời gian đếm ngược
        remainingTime = 60f; // Đặt lại thời gian ban đầu
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Đặt lại các biến quay vòng
        rotationSpeed = RandomRotationSpeed();
        isRight = RandomLeftRight();

        // Đặt lại số lượng chìa khóa
        keyNumber = initialKeyNumber;

        // Hiển thị lại các đối tượng
        spin.gameObject.SetActive(true);
        fruitSpin.rotation = Quaternion.identity; // Reset góc quay về mặc định

        Debug.Log("Game Reset!");

        // Chạy lại các Coroutine
        StartCoroutine(CountDownTime());
        StartCoroutine(CheckDirectionChange());
    }


    public override void StartGame(InteractableScript attachedInteractable = null)
    {
        
        GameManagerSingleton.Player.SpeedMult = 0f;

        if (attachedInteractable != null)
        {
            _attachedInteractable = attachedInteractable;
        }

        gameObject.SetActive(true);
    }


    public override void ExitGame(float delay = 0)
    {
        StartCoroutine(ExitGameRoutine(delay));
    }

    IEnumerator ExitGameRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
        GameManagerSingleton.Player.SpeedMult = 1f;
        ResetGame();
    }


}
