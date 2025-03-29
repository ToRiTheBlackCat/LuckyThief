using Assets.Scripts.StateMachines.Dog;
using UnityEngine;

[System.Serializable]
public class DogScript : MonoBehaviour
{
    #region Components
    private SpriteRenderer _model;
    public Animator _animator { get; private set; }
    private Rigidbody2D _rBody;
    #endregion

    #region State Machine
    [SerializeField] private DogStateMachine _stateMachine;

    public DogIdleState IdleState { get; private set; }
    public DogWalkState WalkState { get; private set; }
    public DogPursuitState PursuitState { get; private set; }
    public DogSleepState SleepState { get; private set; }
    public DogGrabState GrabState { get; private set; }
    #endregion

    [Header("Movement Info")]
    [SerializeField] private Vector2 moveVelocity;
    [SerializeField] private float speed;

    [Header("Inspect Info")]
    public Vector3 HomePosition { get; private set; }
    public Vector3? TargetPosition;// Taget point to go to when thief make a noise
    public ThiefScript TargetThief;
    public Transform AttackCheck { get; private set; }
    private Vector3 attackCheckDir;
    public float detectRange = 10f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rBody = GetComponent<Rigidbody2D>();
        _model = transform.Find("Model").GetComponent<SpriteRenderer>();
        HomePosition = transform.position;

        _stateMachine = new DogStateMachine();
        IdleState = new DogIdleState(this, _stateMachine, "isMoving");
        PursuitState = new DogPursuitState(this, _stateMachine, "isPursuit");
        WalkState = new DogWalkState(this, _stateMachine, "isMoving");
        SleepState = new DogSleepState(this, _stateMachine, "");
        GrabState = new DogGrabState(this, _stateMachine, "isAttack");

        _stateMachine.Initialize(SleepState);

        AttackCheck = transform.Find("AttackCheck");
        attackCheckDir = AttackCheck.position - transform.position;
    }

    private void Update()
    {
        _stateMachine.CurrentState.Update();

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("");
        }
    }

    private void FixedUpdate()
    {
        _rBody.linearVelocity = moveVelocity * Time.fixedDeltaTime;
    }

    public void SetMoveDir(float xAix, float yAxis)
    {
        moveVelocity = new Vector2(xAix, yAxis).normalized * speed;

        if (xAix != 0f)
        {
            _model.flipX = xAix > 0f ? true : false;

            var checkPos = new Vector3(attackCheckDir.x, attackCheckDir.y, 0);
            checkPos.x = checkPos.x * (xAix > 0f ? -1 : 1);
            checkPos += transform.position;

            AttackCheck.position = checkPos;
        }
    }

    public void AnimationFinishTrigger() => _stateMachine.CurrentState.AnimationFinishTrigger();

    public void ExitGrabState()
    {
        if (_stateMachine.CurrentState is DogGrabState)
        {
            _stateMachine.EnterState(IdleState);
        }
    }

    private void OnDrawGizmos()
    {
        if (AttackCheck)
        {
            Gizmos.DrawWireSphere(AttackCheck.position, 3f);
        }

        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var thief = collision.collider.GetComponent<ThiefScript>();

        if (thief != null)
        {
            TargetThief = thief;
            _stateMachine.EnterState(PursuitState);
        }
    }
}
