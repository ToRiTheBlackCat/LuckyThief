using UnityEngine;
using UnityEngine.Events;

public class InteractCheckScript : MonoBehaviour
{
    public UnityEvent<GameObject> OnInteractEnter;

    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask interactableMask;
    public GameObject currentGameObj { get; private set; } = null;

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

    private void Update()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
