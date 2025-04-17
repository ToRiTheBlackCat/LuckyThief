using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace LuckyThief.ThangScripts
{
    public class PlayerCollision : MonoBehaviour
    {

        private void Start()
        {

        }
        private void Update()
        {
            //if (isImpact == true && isTrigger == true && Input.GetKeyUp(KeyCode.E))
            //{
            //    OpenChest();    
            //}
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("MegaBotAmmo"))
            {
                Player player = GetComponent<Player>();
                player.TakeDamage(10f);
            }
            if (collision.CompareTag("MegaBotMissle"))
            {
                Player player = GetComponent<Player>();
                player.TakeDamage(20f);
            }
            //if (collision.CompareTag("Chest"))
            //{
            //    isTrigger = true;
            //    isImpact = true;
            //}
        }
        //public void OpenChest()
        //{

        //    SceneManager.LoadSceneAsync("Wire", LoadSceneMode.Additive);
        //    isTrigger = false;
        //    Debug.Log("Nhấn E để mở rương");
        //}
    }
}
