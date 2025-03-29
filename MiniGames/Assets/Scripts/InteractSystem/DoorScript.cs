using UnityEngine;

public class DoorScript : InteractableScript
{
    private SpriteRenderer _doorPanel;
    private Collider2D _collider;

    [SerializeField] private Sprite _openedSprite;
    [SerializeField] private bool _opened = false;

    private void Awake()
    {
        _doorPanel = transform.Find("Panel").GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void SetHighLight(bool status = true)
    {
        if (!_opened)
        {
            if (_highlightSprite != null)
                _highlightSprite.enabled = status;
            isInteractable = status;

            HoverEvent?.Invoke(status);
        }
    }

    public override void onHandleInteract()
    {
        if (!_opened)
        {
            base.onHandleInteract();
        }
        //if (!isInteractable || _opened)
        //{
        //    return;
        //}

        //if (_attachedGame != null)
        //{
        //    _attachedGame.StartGame(this);
        //}
        //else
        //{
        //    Debug.Log("No attached event.");
        //    OnAttachedMinigameSuccess();
        //}
    }

    public override void OnAttachedMinigameSuccess()
    {
        base.OnAttachedMinigameSuccess();

        _doorPanel.sprite = _openedSprite;
        _collider.enabled = false;
        _opened = true;
    }

    public void SetOpenedStatus(bool status)
    {
        _opened = status;
    }
}
