using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab; 
    [SerializeField] private Transform keySpawnPoint; 
    
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float remainingTime;

    private ShootingKeyAudioController audioController;

    public int keyNumber;
    public Transform spin;
    private bool isGameOver = false; // Add this flag

    private InteractableController interactableController;
    private void Awake()
    {
        audioController = FindAnyObjectByType<ShootingKeyAudioController>();
        interactableController = FindAnyObjectByType<InteractableController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CountDownTime();
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

        interactableController.CloseShootingKeyGame();
    }

    public void GameOver()
    {
        if (isGameOver) return; // Prevent multiple calls

        isGameOver = true;

        // Stop all sounds
        audioController.StopAllSounds();

        audioController.PlayFailSound();
        StartCoroutine(DisapearSpinAnimation());

        Time.timeScale = 0f;
        Debug.Log("Game Over");
        interactableController.CloseShootingKeyGame();
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

        spin.gameObject.SetActive(false);
        interactableController.CloseShootingKeyGame();
    }
    void CountDownTime()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0.0f)
        {
            GameOver();
            return;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);

        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
