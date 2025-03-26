using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

public class InteractableScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer _highlightSprite;

    [SerializeField] protected bool isInteractable;

    //[SerializeField] protected UnityEvent<InteractableScript> InteractEvent;
    [SerializeField] protected MiniGameBase _attachedGame;
    [SerializeField] protected UnityEvent SuccessEvent;

    public void SetHighLight(bool status = true)
    {
        if (_highlightSprite != null)
            _highlightSprite.enabled = status;
        isInteractable = status;
    }

    public virtual void onHandleInteract()
    {
        if (!isInteractable)
        {
            return;
        }

        if (_attachedGame == null)
        {
            Debug.Log("No attached event.");
            return;
        }

        _attachedGame?.StartGame(this);
    }

    private void Start()
    {
        if (_highlightSprite != null)
            _highlightSprite.enabled = false;
        isInteractable = false;
    }

    public virtual void OnAttachedMinigameSuccess()
    {
        SuccessEvent?.Invoke();
    }
}
