using UnityEngine;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class InteractiveController
    {
        public void PlayWireGame()
        {
            LoadWireGame();
        }

        public void CloseMinigameGame()
        {

        }

        public void LoadWireGame()
        {
            SceneManager.LoadSceneAsync("Wire", LoadSceneMode.Additive);
        }

        public void CloseWireGame()
        {
            SceneManager.UnloadSceneAsync("Wire");
        }
    }
}
