using UnityEngine;
using UnityEngine.Events;

public class InteractCheckScript : MonoBehaviour
{
    public UnityEvent<GameObject> OnInteractEnter;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask interactableMask;
    private GameObject currentGameObj = null;

    private void FixedUpdate()
    {
        var position = transform.position;
        var castResult = Physics2D.CircleCast(position, checkRadius, Vector2.zero, 0f, interactableMask);
        //Debug.Log(castResult);

        var gameObject = castResult.collider?.gameObject;
        if (gameObject != null)
        {
            if (currentGameObj != gameObject)
            {
                if (currentGameObj != null)
                {
                    currentGameObj
                        .GetComponent<InteractableScript>()
                        .SetHighLight(false);
                }

                currentGameObj = gameObject;
                var interactable = currentGameObj.GetComponent<InteractableScript>();
                interactable.SetHighLight(true);
            }
            else
            {
                var interactable = currentGameObj.GetComponent<InteractableScript>();
                interactable.SetHighLight(true);
            }
        }
        else
        {
            if (currentGameObj != null)
            {
                currentGameObj
                    .GetComponent<InteractableScript>()
                    .SetHighLight(false);
            }

            currentGameObj = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //OnInteractEnter.Invoke(collision);

        var interact = collision.gameObject.GetComponent<InteractableScript>();

        if (interact != null)
        {
            interact.SetHighLight(true);
            Debug.Log("Interactable collided");
            OnInteractEnter?.Invoke(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var interact = collision.gameObject.GetComponent<InteractableScript>();

        if (interact != null)
        {
            interact.SetHighLight(false);
            Debug.Log("Interactable exited");
        }
    }
}
