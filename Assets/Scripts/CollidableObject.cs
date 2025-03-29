using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CollidableObject : MonoBehaviour
{
    private Collider2D objectCollider;
    [SerializeField] //expose private fields
    private ContactFilter2D filter;
    private List<Collider2D> collidedObjects = new List<Collider2D>(1);

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
            foreach (var o in collidedObjects)
            {
                OnCollided(o.gameObject);
            }
        }
    }

    protected virtual void OnCollided(GameObject collidedObject)
    {
        Debug.Log("Collided with " + collidedObject.name);
    }
}
