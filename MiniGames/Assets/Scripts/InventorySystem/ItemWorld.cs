using Assets.Scripts.InventorySystem;
using UnityEngine;

public class ItemWorld : InteractableScript
{
    public static GameObject itemWorldPrf { get; private set; }
    [SerializeField] private bool isPrefab = false;

    public static InventoryController inventoryInstance;
    [SerializeField] private SpriteRenderer model;
    [SerializeField] private SpriteRenderer highlight;
    [SerializeField] private InventoryItemResource resource;

    [SerializeField] private int amount;

    public override void onHandleInteract()
    {
        if (interactable)
        {
            var addItem = inventoryInstance.AddItem(GetItem());

            if (addItem)
            {
                Debug.Log($"Inventory: Added {amount} {resource.name} to Inventory.");
                Destroy(gameObject);
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
        highlight.enabled = false;

        SetItem(new Item()
        {
            amount = amount,
            resource = resource,
        });
    }

    public void SetItem(Item newItem)
    {
        resource = newItem.resource;
        model.sprite = resource.sprite;
        highlight.sprite = resource.sprite;
        amount = newItem.amount;
    }

    public Item GetItem()
    {
        return new Item()
        {
            resource = this.resource,
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
        itemWorld.resource = spawnItem.resource;
        itemWorld.amount = spawnItem.amount;
    }
}
