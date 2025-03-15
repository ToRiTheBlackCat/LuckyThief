using UnityEngine;

public class MenuController : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
