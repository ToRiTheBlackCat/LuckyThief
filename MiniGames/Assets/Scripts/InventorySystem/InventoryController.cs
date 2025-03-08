using Assets.Scripts.InventorySystem;
using Assets.Scripts.StateMachines;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
public class InventoryController : MonoBehaviour
{
    #region Components
    public Transform throwPoint;
    public ThrowableScript _throwable;
    public ThiefScript thief
    {
        get => GetComponentInParent<ThiefScript>();
    }
    #endregion

    [SerializeField] private GameObject _throwablePrefab;
    [SerializeField] private InventoryItemResource _meatResource;
    [SerializeField] private UI_Inventory inventoryUI;


    [Header("Toolbelt Info")]
    [SerializeField] private List<Item> toolBelt;
    private int maxItemCount = 7;
    public int toolBeltIndex;
    public int TotalWeight
    {
        get
        {
            int totalWeight = 0;
            toolBelt.ForEach(item =>
                totalWeight += (int)item.resource.Weight
            );

            return totalWeight;
        }
    }
    private int maxTotalWeight = 7;
    public int MaxToolIndex => toolBelt.Count() - 1;
    public Item CurrentItem => toolBelt.ElementAt(toolBeltIndex);
    public List<Item> Items => toolBelt;

    [Header("Throw Settings")]
    [SerializeField]
    private float maxThrowDistance;
    [SerializeField] private float throwAngle;
    private const float throwGravity = 9.8f;
    [SerializeField] private LayerMask whatIsObstacle;
    public bool isThrowing;

    // Events
    public UnityEvent ToolIndexChanged;

    private void Awake()
    {
        ItemWorld.inventoryInstance = this;

        toolBelt = new List<Item>()
        {
            new Item()
            {
                resource = _meatResource,
                amount = 2,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 0,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 0,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 0,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 0,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 0,
            },
            new Item()
            {
                resource = _meatResource,
                amount = 2,
            }
        };
    }

    void Start()
    {
        throwPoint = transform.GetChild(0);

        inventoryUI.SetInventory(this);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrowDirection();

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            toolBeltIndex = (int)Mathf.Repeat(
                    toolBeltIndex + (Input.GetAxis("Mouse ScrollWheel") > 0f ? 1 : -1),
                    MaxToolIndex + 1
                );

            ToolIndexChanged?.Invoke();
            inventoryUI.SetInventory(this);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //ProcessCurrentItem();
            isThrowing = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && isThrowing)
        {
            isThrowing = false;
        }

        if (isThrowing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && (thief.stateMachine.currentState is ThiefMovementState))
            {
                var newThrow = Instantiate(_throwablePrefab, GameObject.FindWithTag("ExternalItem").transform, false);
                newThrow.tag = _meatResource.itemName;
                ThrowableScript throwScript = newThrow.GetComponent<ThrowableScript>();
                throwScript.objectSprite = _meatResource.sprite;
                throwScript.LaunchProjectile(transform.position, throwPoint.position, throwAngle);
                //_throwable.LaunchProjectile(transform.position, throwPoint.position, throwAngle);

                thief.stateMachine.EnterState(thief.throwState);
                thief.SetSprite(throwPoint.localPosition.x, 0f);
            }
        }
    }

    public bool AddItem(Item newItem)
    {
        if ((toolBelt.Count == maxItemCount && !newItem.IsStackable()) || (TotalWeight + (int)newItem.weight) > maxTotalWeight)
        {
            Debug.LogError($"Inventory: Can't add new Item. \n" +
                $"Count: ({toolBelt.Count}/{maxItemCount}) \n" +
                $"Weight: ({TotalWeight}/{maxTotalWeight})");
            return false;
        }

        if (newItem.IsStackable())
        {
            var existingType = toolBelt.FirstOrDefault(item =>
                    item.type == newItem.type &&
                    item.Name == newItem.Name
                );
            if (existingType != null)
            {
                var addAmount = newItem.amount;
                existingType.amount += addAmount;
            }
            else
            {
                toolBelt.Add(newItem);
            }

            inventoryUI.SetInventory(this);
        }
        else
        {
            newItem.amount = 1;
            toolBelt.Add(newItem);
            inventoryUI.SetInventory(this);
        }

        return true;
    }

    public bool RemoveItem(Item removeItem, out int amount)
    {
        var existingType = toolBelt.FirstOrDefault(item =>
                    item.type == removeItem.type &&
                    item.Name == removeItem.Name
                );

        if (existingType != null)
        {
            amount = existingType.amount;
            toolBelt.Remove(existingType);
            toolBeltIndex = Mathf.Clamp(toolBeltIndex, 0, MaxToolIndex);
            inventoryUI.SetInventory(this);

            if (amount > 0)
                ItemWorld.SpawnItemIntoWorld(transform.position, existingType);

            return true;
        }

        amount = 0;
        Debug.LogError("Couldn't find item to delete");
        return false;
    }

    private void ProcessThrowDirection()
    {
        var mouseX = Input.GetAxisRaw("Mouse X");
        var mouseY = Input.GetAxisRaw("Mouse Y");

        var mouseMoveDir = new Vector3(mouseX, mouseY, 0);

        throwPoint.position += mouseMoveDir;
        throwPoint.localPosition = Vector3.ClampMagnitude(throwPoint.localPosition, maxThrowDistance);

        var throwObstacle = Physics2D.Raycast(transform.position,
            throwPoint.localPosition,
            throwPoint.localPosition.magnitude,
            whatIsObstacle);

        if (throwObstacle.collider != null)
        {
            throwPoint.position = throwObstacle.point;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(throwPoint.position, .1f);

        // Render throw trajectory
        if (isThrowing)
        {
            Gizmos.DrawLine(transform.position, throwPoint.position);

            var throwDirection = throwPoint.position - transform.position;
            var throwDistance = throwDirection.magnitude;

            var initialSpeed = Mathf.Pow(
                    (throwDistance * throwGravity / Mathf.Sin(2 * throwAngle * Mathf.Deg2Rad)),
                    0.5f
                );

            float prevZAxis = transform.position.y;
            float prevXAxis = transform.position.x;

            for (var time = 0f; time < 100f; time += 0.15f)
            {
                var zAxisNormal = initialSpeed * Mathf.Sin(throwAngle * Mathf.Deg2Rad) * time - 0.5f * throwGravity * Mathf.Pow(time, 2f);
                var xAxisNormal = initialSpeed * Mathf.Cos(throwAngle * Mathf.Deg2Rad) * time;

                // actual position vector of the throwable
                var positionInTime = transform.position + xAxisNormal * throwDirection.normalized;

                var zAxis = positionInTime.y + zAxisNormal; //zAxisNormal is added to have a sense of throw height
                var xAxis = positionInTime.x;

                // Stop drawing when reach the calculated postion of throwable
                if (xAxisNormal >= throwDistance)
                {
                    Gizmos.DrawLine(new Vector3(prevXAxis, prevZAxis, 0), throwDirection + transform.position);
                    break;
                }

                Gizmos.DrawLine(new Vector3(prevXAxis, prevZAxis, 0), new Vector3(xAxis, zAxis, 0));

                prevZAxis = zAxis;
                prevXAxis = xAxis;
            }
        }

    }

    // Handles what to do when pressing use button
    private void ProcessCurrentItem()
    {
        var item = CurrentItem;

        switch (CurrentItem.resource.Type)
        {
            case ItemType.Normal:

                break;
            case ItemType.Throwable:
                isThrowing = true;
                break;
            case ItemType.Consumable:

                break;
            default:
                return;
        }
    }
}

