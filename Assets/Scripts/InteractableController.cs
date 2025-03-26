using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableController : MonoBehaviour
{
    public bool isSuccess = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSafeGame()
    {
        if (!SceneManager.GetSceneByName("SafeDial").isLoaded)
        {
            Theif player = FindFirstObjectByType<Theif>();
            if (player != null)
            {
                player.enabled = false;
            }
            StartCoroutine(LoadSafeDialScene());
        }
    }

    public void ReturnToScene()
    {
        SceneManager.UnloadSceneAsync("SafeDial");
        Theif player = FindFirstObjectByType<Theif>();
        if (player != null)
        {
            player.enabled = true;
        }
    }

    private IEnumerator LoadSafeDialScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SafeDial", LoadSceneMode.Additive);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Set SafeDial as the active scene
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("SafeDial"));

        Camera newCamera = GameObject.FindFirstObjectByType<Camera>();
        if (newCamera != null)
        {
            newCamera.gameObject.SetActive(true);
        }
    }
}
