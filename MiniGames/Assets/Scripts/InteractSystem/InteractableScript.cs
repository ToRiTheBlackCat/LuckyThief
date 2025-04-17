using Assets.Scripts;
using UnityEngine;
using UnityEngine.Events;

[SelectionBase]
public class InteractableScript : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _highlightSprite;

    [SerializeField] protected bool isInteractable;

    [SerializeField] protected UnityEvent<bool> HoverEvent;
    [SerializeField] protected MiniGameBase _attachedGame;
    [SerializeField] protected UnityEvent SuccessEvent;

    public virtual void SetHighLight(bool status = true)
    {
        if (_highlightSprite != null)
            _highlightSprite.enabled = status;
        isInteractable = status;

        HoverEvent?.Invoke(status);
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
            OnAttachedMinigameSuccess();
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
        if (_highlightSprite != null)
            _highlightSprite.enabled = false;
        isInteractable = false;
        HoverEvent?.Invoke(false);

        SuccessEvent?.Invoke();
    }
}
