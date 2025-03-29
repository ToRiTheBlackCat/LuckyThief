using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;
using TMPro;
using System.Threading.Tasks;
using System.Linq;
using Assets.Scripts;

namespace LuckyThief.VinhScripts
{

	public class Fixed00Interactable : MiniGameBase
	{
		public GameObject interactIcon;
		public GameObject minigameUI;
		public GameObject messageObject;
		public GameObject reward;
		public TMP_Text messageText;
		[SerializeField]
		bool state2;
		[SerializeField]
		bool minigameCleared;
		Fixed00 dOhScript;
		public Collider2D objectCollider;
		[SerializeField] //expose private fields
		private ContactFilter2D filter;
		public List<Collider2D> collidedObjects = new List<Collider2D>(2);

		protected void Start()
		{
			objectCollider = GetComponent<Collider2D>();

			//minigameUI = GameObject.Find("CanvasMinigame/doubleOh");
			//interactIcon = GameObject.Find("spacebarIcon");
			//messageObject = GameObject.Find("Message");
			messageText = messageObject.GetComponent<TMP_Text>();
			dOhScript = minigameUI.GetComponentInChildren<Fixed00>();
			reward.SetActive(false);

			messageObject.SetActive(false);
			interactIcon.SetActive(false);
			minigameUI.SetActive(false);

			state2 = false;
			minigameCleared = false;
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
			if (dOhScript.isSolved && !minigameCleared)
			{
				OnMinigameSolved();
			}
		}

		protected void OnNoCollision()
		{
			interactIcon.SetActive(false);
			minigameUI.SetActive(false);
			state2 = false;
		}

		protected async void OnMinigameSolved()
		{
			minigameCleared = true;
			messageObject.SetActive(true);
			messageText.text = "YOU SOLVED THE MINIGAME!";
			await Task.Delay(5000);
			reward.SetActive(true);
			OnEscapeInteract();
		}

		protected void OnCollision(GameObject collidedObject)
		{
			interactIcon.SetActive(true);
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
			StartGame();
		}

		private void OnEscapeInteract()
		{
			//Debug.Log("Open Minigame UI");
			messageObject.SetActive(false);
			ExitGame();
			state2 = false;
		}

        public override void StartGame(InteractableScript attachedInteractable = null)
        {
            //Debug.Log("Open Minigame UI");
            minigameUI.SetActive(true);
        }

        public override void ExitGame(float delay = 0)
        {
            minigameUI.SetActive(false);
        }
    }

        
}