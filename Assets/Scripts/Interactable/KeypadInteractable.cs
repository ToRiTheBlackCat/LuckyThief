using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;
using TMPro;
using System.Threading.Tasks;
namespace LuckyThief.VinhScripts
{
    public class KeypadInteractable : MonoBehaviour
    {
        public GameObject minigameUI;
        public GameObject passwordHolderObj;
        public GameObject interactUI;
        public GameObject messageObject;
        public GameObject noteUI;
        public GameObject reward;
        public TMP_Text messageText;
        Keypad keypadScript;
        KeypadPasswordInteractable keypadNoteScript;
        [SerializeField]
        bool state2;
        [SerializeField]
        bool minigameCleared;
        [SerializeField]
        bool passwordFound;
        bool toggleNote;
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
            keypadScript = minigameUI.GetComponentInChildren<Keypad>();
            keypadNoteScript = passwordHolderObj.GetComponentInChildren<KeypadPasswordInteractable>();

            messageObject.SetActive(false);
            interactUI.SetActive(false);
            minigameUI.SetActive(false);
            noteUI.SetActive(false);
            reward.SetActive(false);

            state2 = false;
            minigameCleared = false;
            toggleNote = false;
        }

        protected void Update()
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
            passwordFound = keypadNoteScript.toggleCollected;
            if (passwordFound && !toggleNote)
            {
                FoundPasswordNote();
            }
            if (keypadScript.isSolved && !minigameCleared)
            {
                OnMinigameSolved();
            }
        }

        protected void FoundPasswordNote()
        {
            noteUI.SetActive(true);
            toggleNote = true;
        }

        protected void OnNoCollision()
        {
            //Debug.Log("OnNoCollision running");
            interactUI.SetActive(false);
            messageObject.SetActive(false);
            minigameUI.SetActive(false);
            state2 = false;
        }

        protected async void OnMinigameSolved()
        {
            //Debug.Log("OnMiniGameSolved running");
            minigameCleared = true;
            messageObject.SetActive(true);
            messageText.text = "YOU SOLVED THE MINIGAME!";
            await Task.Delay(5000);
            reward.SetActive(true);
            OnEscapeInteract();
        }

        protected void OnCollision(GameObject collidedObject)
        {
            //Debug.Log("OnCollision running");
            interactUI.SetActive(true);
            //Technically check this then never check again
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnInteract();
                state2 = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape) && state2)
            {
                OnEscapeInteract();
            }
        }

        private void OnInteract()
        {
            //Debug.Log("Open Minigame UI");
            minigameUI.SetActive(true);
        }

        private void OnEscapeInteract()
        {
            //Debug.Log("Open Minigame UI");
            messageObject.SetActive(false);
            minigameUI.SetActive(false);
            state2 = false;
        }
    } 
}
