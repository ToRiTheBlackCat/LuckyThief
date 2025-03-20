using Assets.Scripts.InventorySystem;
using Assets.Scripts.StateMachines;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.Events;

#nullable enable
public class InventoryController : MonoBehaviour
{
    #region Components
    private Transform throwPoint;
    private UI_Inventory _inventoryUI = null!;
    [HideInInspector] public ThiefScript _thief = null!;
    #endregion

    [SerializeField] private InventoryItemResource _meatResource;
    [SerializeField] private GameObject _throwablePrefab;


    [Header("Toolbelt Info")]
    [SerializeReference] private List<Item> toolBelt;
    [SerializeField] public int ItemCountMax { get; private set; } = 7;
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
    public int TotalWeightMax { get; private set; } = 7;
    public float WeightRatio => TotalWeight * 1f / TotalWeightMax;
    public int ToolIndexCurrent { get; private set; }
    public int ToolIndexMax => toolBelt.Count() - 1;
    public Item? CurrentItem => toolBelt.Count > 0 ? toolBelt.ElementAt(ToolIndexCurrent) : null;
    public List<Item> Items => toolBelt.ToList();

    [Header("Throw Settings")]
    [SerializeField] private float throwDistanceMax;
    [SerializeField] private float throwAngle;
    private const float throwGravity = 9.8f;
    [SerializeField] private LayerMask whatIsObstacle;
    private bool _isThrowing;

    // Events
    public UnityEvent<int> ToolIndexChanged = null!;
    public UnityEvent ItemChanged = null!;

    private void Awake()
    {
        ItemWorld.inventoryInstance = this;

        throwPoint = transform.GetChild(0);
        _inventoryUI = GameObject.Find("UI_Inventory").GetComponent<UI_Inventory>();

        toolBelt = new List<Item>();
        #region Init temp Items
        //toolBelt = new List<Item>()
        //{
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 2,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 0,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 0,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 0,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 0,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 0,
        //    },
        //    new Item()
        //    {
        //        resource = _meatResource,
        //        amount = 2,
        //    }
        //}; 
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrowDirection();

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            ToolIndexCurrent = (int)Mathf.Repeat(
                    ToolIndexCurrent + (Input.GetAxis("Mouse ScrollWheel") > 0f ? 1 : -1),
                    ToolIndexMax + 1
                );

            ToolIndexChanged.Invoke(ToolIndexCurrent);
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ProcessRightClick();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _isThrowing = false;
        }

        if (_isThrowing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && (_thief.stateMachine.currentState is ThiefMovementState))
            {
                ThrowItem();
            }
        }
    }

    // Handles what to do when pressing use button
    private void ProcessRightClick()
    {
        if (CurrentItem == null)
        {
            Debug.Log("Toolbelt is empty");
            return;
        }

        switch (CurrentItem.resource.Type)
        {
            case ItemType.Normal:

                break;
            case ItemType.Throwable:
                _isThrowing = true;
                break;
            case ItemType.Consumable:

                break;
            default:
                return;
        }
    }

    private void ThrowItem()
    {
        if (CurrentItem!.type != ItemType.Throwable)
        {
            return;
        }

        var resource = CurrentItem!.resource;
        Item removeItem = new Item()
        {
            resource = CurrentItem!.resource,
            amount = 1,
        };

        int removeAmount = 0;
        var removal = RemoveItem(removeItem, out removeAmount);

        if (removal && removeAmount > 0)
        {
            var newThrow = Instantiate(_throwablePrefab, GameObject.FindWithTag("ExternalItem").transform, false);
            newThrow.tag = _meatResource.itemName;
            ThrowableScript throwScript = newThrow.GetComponent<ThrowableScript>();
            throwScript.objectSprite = _meatResource.sprite;
            throwScript.LaunchProjectile(transform.position, throwPoint.position, throwAngle);

            _thief.stateMachine.EnterState(_thief.throwState);
            _thief.SetSprite(throwPoint.localPosition.x, 0f);
            _isThrowing = false;
        }
    }

    #region Toolbelt function
    public bool AddItem(Item newItem)
    {
        if (CanAddItem(newItem))
        {
            Debug.LogError($"Inventory: Can't add new Item. \n" +
                $"Count: ({toolBelt.Count}/{ItemCountMax}) \n" +
                $"Weight: ({TotalWeight}/{TotalWeightMax})");
            _inventoryUI.SetInventory(this);
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
        }
        else
        {
            newItem.amount = 1;
            toolBelt.Add(newItem);
        }

        ToolIndexCurrent = Math.Clamp(ToolIndexCurrent, 0, ToolIndexMax);
        _inventoryUI.SetInventory(this);
        return true;
    }
    public bool CanAddItem(Item newItem)
    {
        return (toolBelt.Count == ItemCountMax && !newItem.IsStackable()) || (TotalWeight + (int)newItem.weight) > TotalWeightMax;
    }

    public bool RemoveItem(Item removeItem, out int amount)
    {
        var existingType = toolBelt.FirstOrDefault(item =>
                    item.type == removeItem.type &&
                    item.Name == removeItem.Name
                );

        amount = 0;
        if (existingType != null)
        {
            var removeAmount = removeItem.amount;
            if (existingType.amount > removeAmount)
            {
                existingType.amount -= removeAmount;
                amount = removeAmount;
            }
            else
            {
                amount = existingType.amount;
                toolBelt.Remove(existingType);
            }

            ToolIndexCurrent = Mathf.Clamp(ToolIndexCurrent, 0, ToolIndexMax);
            _inventoryUI.SetInventory(this);

            return true;
        }

        Debug.LogError("Couldn't find item to delete");
        return false;
    }
    #endregion

    private void ProcessThrowDirection()
    {

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        var clampedVec = Vector3.ClampMagnitude(mousePos - transform.position, throwDistanceMax);
        throwPoint.position = transform.position + clampedVec;

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
        if (throwPoint == null)
            return;

        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Gizmos.DrawWireSphere(mousePos, .1f);

        Gizmos.DrawSphere(throwPoint.position, .1f);

        // Render throw trajectory
        if (_isThrowing)
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


}

