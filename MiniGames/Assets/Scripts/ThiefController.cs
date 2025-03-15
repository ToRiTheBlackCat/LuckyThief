using Assets.Scripts;
using Assets.Scripts.StateMachines;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class ThiefScript : MonoBehaviour
{
    #region Components
    public Rigidbody2D _rBody;
    [SerializeField] private Camera _camera;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private SpriteRenderer _shadowRenderer;
    [field: SerializeField]public Animator Animator { get; private set; }
    [SerializeField] private NoiseController _noiseController;
    [SerializeField] private PlayerUIScript PlayerUI;

    private InteractCheckScript _interactCheck;
    #endregion

    #region Animation Info
    private int isMovingHash = Animator.StringToHash("isMoving");
    private int throwHash = Animator.StringToHash("throw");
    #endregion

    [Header("Input info")]
    public float xAxis;
    public float yAxis;

    [Header("Movement info")]
    public float Speed = 10;
    public Vector2 Velocity;

    #region Interact area Settings
    [Header("Interact area Settings")]
    [SerializeField] private LayerMask _interactCheckMask;
    [SerializeField] private float interactDistance;
    #endregion


    #region States
    public ThiefStateMachine stateMachine;

    public ThiefIdleState idleState;
    public ThiefWalkState walkState;
    public ThiefThrowState throwState;
    #endregion

    private void Awake()
    {
        _rBody = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
        _noiseController = GetComponent<NoiseController>();
        _noiseController.onNoiseChange.AddListener(x => PlayerUI.OnNoiseControllerNoiseChange(x));
        _noiseController.onThreshold.AddListener(PlayerUI.OnNoiseControllerThreshold);

        #region Init StateMachine
        stateMachine = new ThiefStateMachine();

        idleState = new ThiefIdleState(this, stateMachine, "Idle");
        walkState = new ThiefWalkState(this, stateMachine, "Walk");
        throwState = new ThiefThrowState(this, stateMachine, "Throw");

        stateMachine.Initialize(idleState);
        #endregion

        _interactCheck = GetComponentInChildren<InteractCheckScript>();
    }

    void Start()
    {
        GameManagerSingleton.Player = this;

        //_interactCheck = GetComponentInChildren<InteractCheckScript>();
        //_interactCheck.OnInteractEnter.AddListener(x => OnInteractCheckCollisionEnter(x));

        var animTriggers = GetComponentInChildren<ThiefAnimationTriggers>();

        //Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        CameraFollow();
    }

    private void FixedUpdate()
    {
        ProcessMovement();
        stateMachine.currentState.Update();
    }

    public float followAccel = 0.03f;
    public float maxFollowDistance = 3;
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

    }

    public void SetVelocity(float xAxis, float yAxis)
    {
        var direction = new Vector2(xAxis, yAxis);
        _rBody.linearVelocity = direction.normalized * Speed * Time.deltaTime;
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
