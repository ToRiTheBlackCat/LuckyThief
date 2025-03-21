using UnityEngine;

public class DoorScript : InteractableScript
{
    private SpriteRenderer _doorPanel;
    private Collider2D _collider;

    [SerializeField] private Sprite _openedSprite;
    private bool _opened = false;

    private void Awake()
    {
        _doorPanel = transform.Find("Panel").GetComponent<SpriteRenderer>();
        _collider = GetComponent<BoxCollider2D>();
    }

    public override void onHandleInteract()
    {
        if (!isInteractable || _opened)
        {
            return;
        }

        if (InteractEvent == null)
        {
            Debug.Log("No attached event.");
            return;
        }

        if (InteractEvent.GetPersistentEventCount() > 0)
        {
            InteractEvent.Invoke(this);
        }
        else
        {
            OnAttachedMinigameSuccess();
        }
    }

    public override void OnAttachedMinigameSuccess()
    {
        _doorPanel.sprite = _openedSprite;
        _collider.enabled = false;
        _opened = true;
    }
}
