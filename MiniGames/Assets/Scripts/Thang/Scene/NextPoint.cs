using UnityEngine;
namespace LuckyThief.ThangScripts { 
    public class NextPoint : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] public audioManager audioManager;
        private bool isTrigger;

        private void Update()
        {
            if (isTrigger == true && Input.GetKeyUp(KeyCode.F))
            {
                LoadBossScene();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                isTrigger = true;
            }
        }
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //    {
        //        isTrigger = false;
        //    }
        //}
        private void LoadBossScene()
        {
            SceneController.instance.NextLevel();
            audioManager.StopMusic();
            isTrigger = false;
        }
    }
}