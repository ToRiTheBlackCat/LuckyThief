using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableController : MonoBehaviour
{
    public bool isSuccess = false;
    private bool isNearItem = false;


    public string minigameName;

    private void Update()
    {
        // Check if player is near the item and presses "F"
        if (isNearItem && !isSuccess && Input.GetKeyDown(KeyCode.F))
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
    }

    public void CloseGame()
    {
        SceneManager.UnloadSceneAsync(minigameName);
    }
}
