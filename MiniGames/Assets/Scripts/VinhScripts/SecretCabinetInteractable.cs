using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;
using TMPro;
using System.Threading.Tasks;

namespace LuckyThief.VinhScripts
{
    public class SecretCabinetInteractable : MonoBehaviour
    {
        public GameObject interactUI;
        public GameObject messageObject;
        public GameObject closedCabinet;
        public GameObject openedCabinet;
        public TMP_Text messageText;
        public bool toggleInteracted;
        public KeyCode interactKey;
        public Collider2D objectCollider;
        [SerializeField] //expose private fields
        private ContactFilter2D filter;
        public List<Collider2D> collidedObjects = new List<Collider2D>(1);

        protected void Start()
        {
            interactKey = KeyCode.Space;
            objectCollider = GetComponent<Collider2D>();
            messageText = messageObject.GetComponent<TMP_Text>();

            messageObject.SetActive(false);
            interactUI.SetActive(false);
            closedCabinet.SetActive(true);
            openedCabinet.SetActive(false);

            toggleInteracted = false;
        }

        protected void Update()
        {
            if (!toggleInteracted)
            {
                if (objectCollider != null)
                {
                    objectCollider.Overlap(filter, collidedObjects);
                    if (collidedObjects.Count > 0 && collidedObjects[0].CompareTag("PlayerUI"))
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
            interactUI.SetActive(false);
        }

        protected void OnCollision(GameObject collidedObject)
        {
            interactUI.SetActive(true);
            //Technically check this then never check again
            if (Input.GetKeyDown(interactKey))
            {
                OnInteract();
            }
        }

        protected async void OnInteract()
        {
            toggleInteracted = true;
            messageText.text = "A Cabinet slides out";
            messageObject.SetActive(true);
            closedCabinet.SetActive(false);
            openedCabinet.SetActive(true);
            await Task.Delay(3000);
            messageObject.SetActive(false);
            interactUI.SetActive(false);
        }

    }
}