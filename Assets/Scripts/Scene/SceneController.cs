using UnityEngine;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class SceneController : MonoBehaviour
    {
        public static SceneController instance;
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void NextLevel()
        {
            SceneManager.LoadSceneAsync("BossMap");

        }
        public void LoadScene(string name)
        {
            SceneManager.LoadSceneAsync(name);
        }
    }
}
