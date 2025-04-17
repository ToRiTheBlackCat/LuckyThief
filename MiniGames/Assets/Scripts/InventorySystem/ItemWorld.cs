using Assets.Scripts.InventorySystem;
using Mono.Cecil;
using System;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[ExecuteInEditMode]
public class ItemWorld : InteractableScript
{
    public static GameObject itemWorldPrf { get; private set; }
    [SerializeField] private bool isPrefab = false;

    public static InventoryController inventoryInstance;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] private InventoryItemResource _resource;
    public InventoryItemResource Resource
    {
        get => _resource;
        set
        {
            if (value == null)
                return;

            _resource = value;

            model = transform.Find("Model").GetComponent<SpriteRenderer>();
            highlight = transform.Find("Highlight").GetComponent<SpriteRenderer>();
            model.sprite = _resource.sprite;
            highlight.sprite = _resource.sprite;
            gameObject.name= _resource.name;
        }
    }

    [SerializeField] private int amount;

    private void OnValidate()
    {
        Resource = _resource;
    }

    public override void onHandleInteract()
    {
        if (isInteractable)
        {
            var addItem = inventoryInstance.AddItem(GetItem());

            if (addItem)
            {
                //var thief = inventoryInstance._thief;
                //thief.stateMachine.EnterState(thief.takeState);

                Debug.Log($"Inventory: Added {amount} {Resource.name} to Inventory.");
                Destroy(gameObject);
                OnAttachedMinigameSuccess();
            }
        }

        return;
    }

    private void Awake()
    {
        if (isPrefab)
            itemWorldPrf ??= gameObject;

        model = transform.Find("Model").GetComponent<SpriteRenderer>();
        highlight = transform.Find("Highlight").GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (!Application.isPlaying)
            return;

        highlight.enabled = false;
    }

    public void SetItem(Item newItem)
    {
        _resource = newItem.resource;
        model.sprite = Resource.sprite;
        highlight.sprite = Resource.sprite;
        amount = newItem.amount;
    }

    public Item GetItem()
    {
        return new Item()
        {
            resource = this.Resource,
            amount = this.amount,
        };
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public static void SpawnItemIntoWorld(Vector3 postion, Item spawnItem)
    {
        Transform parent = ItemWorld.itemWorldPrf.transform.parent.GetComponent<Transform>();
        Transform transform = Instantiate(ItemWorld.itemWorldPrf, parent).GetComponent<Transform>();
        transform.position = postion;

        ItemWorld itemWorld = transform.GetComponent<ItemWorld>();
        itemWorld.Resource = spawnItem.resource;
        itemWorld.amount = spawnItem.amount;
    }
}
