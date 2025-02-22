using Assets.Scripts.StateMachines;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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


    [Header("Toolbelt Info")]
    [SerializeField] private IEnumerable<InventoryItemResource> toolBelt = new List<InventoryItemResource>();
    [SerializeField] private int toolBeltIndex;
    public int MaxToolIndex
    {
        get
        {
            return toolBelt.Count() - 1;
        }
    }
#nullable enable
    public InventoryItemResource? CurrentItem
    {
        get
        {
            return toolBelt.ElementAt(toolBeltIndex);
        }
    }

    [Header("Throw Settings")]
    [SerializeField] private float maxThrowDistance;
    [SerializeField] private float throwAngle;
    private const float throwGravity = 9.8f;
    [SerializeField] private LayerMask whatIsObstacle;
    public bool isThrowing;

    // Events
    public UnityEvent ToolIndexChanged;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        throwPoint = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrowDirection();

        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            // Set toolBelt index base on mouse scroll direction
            toolBeltIndex = Mathf.Clamp(
                    toolBeltIndex + (Input.GetAxis("Mouse ScrollWheel") > 0f ? 1 : -1),
                    0,
                    MaxToolIndex
                );

            ToolIndexChanged?.Invoke();
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
                ThrowableScript throwScript = newThrow.GetComponent<ThrowableScript>();
                throwScript.objectSprite = _meatResource.sprite;
                throwScript.LaunchProjectile(transform.position, throwPoint.position, throwAngle);
                //_throwable.LaunchProjectile(transform.position, throwPoint.position, throwAngle);

                thief.stateMachine.EnterState(thief.throwState);
                thief.SetSprite(throwPoint.localPosition.x, 0f);
            }
        }
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

        switch (CurrentItem.type)
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

