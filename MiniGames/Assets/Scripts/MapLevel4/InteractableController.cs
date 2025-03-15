using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableController : MonoBehaviour
{
    public bool isSuccess;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLockPickGame()
    {
        SceneManager.LoadSceneAsync("LockPickGame", LoadSceneMode.Additive);

    }

    public void CloseLockPickGame()
    {
        SceneManager.UnloadSceneAsync("LockPickGame");
    }

    public void LoadShootingKeyGame()
    {
        SceneManager.LoadSceneAsync("ShootingKeyGame", LoadSceneMode.Additive);

    }

    public void CloseShootingKeyGame()
    {
        SceneManager.UnloadSceneAsync("ShootingKeyGame");
    }

}
