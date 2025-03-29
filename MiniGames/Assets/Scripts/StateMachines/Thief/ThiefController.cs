using Assets.Scripts;
using Assets.Scripts.StateMachines;
using Assets.Scripts.StateMachines.Thief;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[Serializable]
public class ThiefScript : MonoBehaviour
{
    #region Components
    private Rigidbody2D _rBody;
    [SerializeField] private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer _shadowRenderer;
    public Animator _animator { get; private set; }
    private NoiseController _noiseController;
    private InventoryController _inventory;
    private PlayerUIScript _playerUI;

    private InteractCheckScript _interactCheck;
    public InteractCheckScript InteractCheck { get => _interactCheck; }
    public InventoryController Inventory { get => _inventory; }
    #endregion

    #region Animation Info
    private int isMovingHash = Animator.StringToHash("isMoving");
    private int throwHash = Animator.StringToHash("throw");
    #endregion

    [Header("Movement info")]
    public float xAxis;
    public float yAxis;
    [Space]
    public float Speed = 10;
    public Vector2 Velocity;

    #region Interact area Settings
    [Header("Interact area Settings")]
    //[SerializeField] private LayerMask _interactCheckMask;
    [SerializeField] private float interactDistance;
    #endregion


    #region States
    public ThiefStateMachine stateMachine;

    public ThiefIdleState idleState { get; private set; }
    public ThiefWalkState walkState { get; private set; }
    public ThiefThrowState throwState { get; private set; }
    public ThiefTakeState takeState { get; private set; }
    public ThiefGrabbedState grabbedState { get; private set; }
    #endregion

    [Header("Camera Settings")]
    public float followAccel = 0.03f;
    public float maxFollowDistance = 3;

    private void Awake()
    {
        _camera = Camera.main;
        _playerUI = GameObject.Find("PlayerUI").GetComponent<PlayerUIScript>();
        _rBody = GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.Find("Model").GetComponent<SpriteRenderer>();
        _shadowRenderer = transform.Find("Shadow").GetComponent<SpriteRenderer>();

        _animator = GetComponentInChildren<Animator>();
        _noiseController = GetComponent<NoiseController>();
        _noiseController.onThreshold.AddListener(_playerUI.OnNoiseControllerThreshold);
        _noiseController.onNoiseChange.AddListener(x => _playerUI.OnNoiseControllerNoiseChange(x));


        #region Init StateMachine
        stateMachine = new ThiefStateMachine();

        idleState = new ThiefIdleState(this, stateMachine, "Idle");
        walkState = new ThiefWalkState(this, stateMachine, "Walk");
        throwState = new ThiefThrowState(this, stateMachine, "Throw");
        takeState = new ThiefTakeState(this, stateMachine, "TakeItem");
        grabbedState = new ThiefGrabbedState(this, stateMachine, "isGrabbed");

        stateMachine.Initialize(idleState);
        #endregion
        _interactCheck = GetComponentInChildren<InteractCheckScript>();
        _inventory = GetComponentInChildren<InventoryController>();
        _inventory._thief = this;
    }

    void Start()
    {
        GameManagerSingleton.Player = this;
    }


    void Update()
    {
        CameraFollow();
        stateMachine.currentState.Update();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
    }

    private void CameraFollow()
    {
        if (_camera == null)
        {
            return;
        }

        var camTransform = _camera.transform;
        var followPosition = new Vector3(transform.position.x, transform.position.y, camTransform.position.z);

        camTransform.position = Vector3.Lerp(camTransform.position, followPosition, followAccel);
    }


    private void ProcessMovement()
    {
        var forwardPress = Input.GetKey(KeyCode.W);
        var backwardPress = Input.GetKey(KeyCode.S);
        var leftPress = Input.GetKey(KeyCode.A);
        var rightPress = Input.GetKey(KeyCode.D);

        var verticalConcurrent = (forwardPress && backwardPress) || (!forwardPress && !backwardPress);
        var horizontalConcurrent = (leftPress && rightPress) || (!leftPress && !rightPress);

        var direction = Vector2.zero;
        direction.y = !verticalConcurrent ? (forwardPress ? 1 : -1) : 0;
        direction.x = !horizontalConcurrent ? (rightPress ? 1 : -1) : 0;
        xAxis = direction.x;
        yAxis = direction.y;

        _rBody.linearVelocity = Velocity * Time.fixedDeltaTime;
    }

    [HideInInspector] public float SpeedMult = 1f;
    public void SetVelocity(float xAxis, float yAxis)
    {
        var direction = new Vector2(xAxis, yAxis);
        var speedPenalty = 0f;
        if (_inventory.WeightRatio >= .5f)
            speedPenalty = (Mathf.Clamp(_inventory.WeightRatio, 0, 75f) - .5f) * Speed;


        //_rBody.linearVelocity = direction.normalized * (Speed - speedPenalty) * Time.fixedDeltaTime;
        Velocity = direction.normalized * (Speed - speedPenalty);
        Velocity *= SpeedMult;
        if (Velocity.x != 0)
            SetSprite(xAxis, yAxis);
    }

    public void SetSprite(float xVel, float yVel)
    {
        if (xVel != 0)
        {
            var flip = xVel > 0 ? false : true;
            _spriteRenderer.flipX = flip;
            _shadowRenderer.flipX = flip;

            var position = _interactCheck.transform.localPosition;
            position.x = interactDistance * Mathf.Sign(xVel);
            _interactCheck.transform.localPosition = position;
        }
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
}
