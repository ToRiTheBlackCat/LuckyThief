using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableController : MonoBehaviour
{
    public bool isSuccess = false;
    private bool isNearItem = false;


    public string minigameName;
    private bool isMinigameLoaded;
    [SerializeField] private GameObject RewardItem;
    [SerializeField] private GameObject AnnimationCoin;

    private void Awake()
    {
        RewardItem.SetActive(false);
    }

    private void Update()
    {
        // Check if player is near the item and presses "F"
        if (isNearItem && !isMinigameLoaded && !isSuccess && Input.GetKeyDown(KeyCode.F))
        {
            LoadGame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the item area: " + other.gameObject.name);
            isNearItem = true; // Mark that the player is near the item
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left the item's area.");
            isNearItem = false; // Mark that the player is no longer near the item
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(minigameName, LoadSceneMode.Additive);
        isMinigameLoaded = true;
    }

    public void CloseGame()
      {
        isMinigameLoaded = false;
        SceneManager.UnloadSceneAsync(minigameName);


        Debug.Log("CloseGame called!");

        if (isSuccess)
        {
            RewardItem.SetActive(true);
            AnnimationCoin.SetActive(false);
        }
    }
}
