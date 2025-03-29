using UnityEngine;
namespace LuckyThief.ThangScripts { 
    public class NextPoint : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] public audioManager audioManager;
        private bool isTrigger;

        private void Update()
        {
            if (isTrigger == true && Input.GetKeyUp(KeyCode.E))
            {
                LoadBossScene();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") )
            {
                isTrigger = true;
            }
        }
        private void LoadBossScene()
        {
            SceneController.instance.NextLevel();
            audioManager.StopMusic();
        }
    }
}