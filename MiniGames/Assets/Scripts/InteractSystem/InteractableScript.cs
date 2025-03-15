using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class InteractableScript : MonoBehaviour
{
    public UnityEvent InteractEvent;

    [SerializeField] SpriteRenderer _highlightSprite;

    [SerializeField] protected bool interactable;

    public void SetHighLight(bool status = true)
    {
        if (_highlightSprite != null)
            _highlightSprite.enabled = status;
        interactable = status;
    }

    public virtual void onHandleInteract()
    {
        if (!interactable)
        {
            return;
        }

        if (InteractEvent == null)
        {
            Debug.Log("No attached event.");
            return;
        }

        InteractEvent?.Invoke();
    }

    private void Start()
    {
        if (_highlightSprite != null)
            _highlightSprite.enabled = false;
        interactable = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            onHandleInteract();
        }
    }
}
