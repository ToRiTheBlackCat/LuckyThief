using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;
using TMPro;
using System.Threading.Tasks;

namespace LuckyThief.VinhScripts
{
    public class CollectableInteractable : MonoBehaviour
    {
        public GameObject interactUI;
        public GameObject messageObject;
        public TMP_Text messageText;
        public bool toggleCollected;
        public string itemName;

        public Collider2D objectCollider;
        [SerializeField] //expose private fields
        private ContactFilter2D filter;
        public List<Collider2D> collidedObjects = new List<Collider2D>(1);

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            objectCollider = GetComponent<Collider2D>();

            //minigameUI = GameObject.Find("CanvasMinigame/keypad");
            //interactUI = GameObject.Find("spacebarIcon");
            //messageObject = GameObject.Find("Message");
            messageText = messageObject.GetComponent<TMP_Text>();

            messageObject.SetActive(false);
            interactUI.SetActive(false);

            toggleCollected = false;
        }

        protected void Update()
        {
            if (!toggleCollected)
            {
                if (objectCollider != null)
                {
                    objectCollider.Overlap(filter, collidedObjects);
                    if (collidedObjects.Count > 0 && collidedObjects[0].CompareTag("Player"))
                    {
                        foreach (var o in collidedObjects)
                        {
                            OnCollision(o.gameObject);
                        }
                    }
                    else
                    {
                        OnNoCollision();
                    }
                }
            }
        }

        protected void OnNoCollision()
        {
            //Debug.Log("OnNoCollision running");
            interactUI.SetActive(false);
        }


        protected void OnCollision(GameObject collidedObject)
        {
            //Debug.Log("OnCollision running");
            interactUI.SetActive(true);
            //Technically check this then never check again
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnInteract();
            }
        }

        private async void OnInteract()
        {
            toggleCollected = true;
            messageText.text = $"Found {itemName}";
            messageObject.SetActive(true);
            await Task.Delay(3000);
            messageObject.SetActive(false);
            interactUI.SetActive(false);
        }
    }

}