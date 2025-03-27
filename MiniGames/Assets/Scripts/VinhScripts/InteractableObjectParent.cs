using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Compilation;

namespace LuckyThief.VinhScripts
{
	public class InteractableObjectParent : MonoBehaviour
	{
		public KeyCode interactKey;
		public KeyCode escapeInteractKey;
		public Collider2D objectCollider;
		[SerializeField] //expose private fields
		private ContactFilter2D filter;
		public List<Collider2D> collidedObjects = new List<Collider2D>(1);
		bool toggleFlag;

		protected virtual void Start()
		{
			objectCollider = GetComponent<Collider2D>();
			if (objectCollider == null)
			{
				Debug.LogError("Collider2D component is missing on this GameObject.");
			}
		}

		protected virtual void Update()
		{
			if (objectCollider != null)
			{
				objectCollider.Overlap(filter, collidedObjects);
				if (collidedObjects.Count > 0)
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

		protected virtual void OnNoCollision()
		{

		}

		protected virtual void OnCollision(GameObject collidedObject)
		{
			//Technically check this then never check again
			if (Input.GetKeyDown(interactKey))
			{
				OnInteract();
			}
			if (Input.GetKeyDown(escapeInteractKey) && toggleFlag)
			{
				OnEscapeInteract();
			}
		}

		protected virtual void OnInteract()
		{
			toggleFlag = true;
		}

		protected virtual void OnEscapeInteract()
		{
			toggleFlag = false;
		}
	}

}