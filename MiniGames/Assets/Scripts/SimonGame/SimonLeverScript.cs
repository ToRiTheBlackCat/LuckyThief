using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SimonLeverScript : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private SimonSaysScript _simon;

    private float maxUpAngle = 45f;
    private float maxDownAngle = 45f;
    private Quaternion defaultGbRotation;

    private bool mouseHover = false;
    private bool mousePressed = false;
    [SerializeField] private Vector2 prevMousePos;
    [SerializeField] private Vector2 mouseDir;

    private bool canFlip = true;

    //Events
    public UnityEvent<string> OnLeverFlipped;

    // Coroutines
    private Coroutine RotateLeverCoroutine;

    // Input names
    public string LeverDown { get; private set; } = "LeverDown";
    public string LeverUp { get; private set; } = "LeverUp";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _simon = GetComponentInParent<SimonSaysScript>();
        OnLeverFlipped.AddListener(_simon.OnButtonPressed);

        LeverDown = gameObject.name + "Down";
        LeverUp = gameObject.name + "Up";
        var append = _simon.availableInputs.Append(LeverUp);
        append = append.Append(LeverDown);
        _simon.availableInputs = append.ToArray();

        defaultGbRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (mousePressed)
        {
            var convertedMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePos = new Vector2(convertedMouse.x, convertedMouse.y);
            mouseDir = mousePos - prevMousePos;

            prevMousePos = mousePos;

            RotateLever(mouseDir, .8f);
        }
    }

    #region MouseInputs
    private void OnMouseEnter()
    {
        if (_simon.allowInput)
        {
            mouseHover = true;
        }
    }

    private void OnMouseExit()
    {
        if (mouseHover)
        {
            mouseHover = !mouseHover;
        }
    }

    private void OnMouseDown()
    {
        if (!mousePressed && mouseHover && _simon.allowInput)
        {
            mousePressed = true;
            prevMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseUp()
    {
        if (mousePressed)
        {
            mousePressed = !mousePressed;
        }
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="duration"></param>
    /// <param name="returnDuration"></param>
    private void RotateLever(Vector2 direction, float duration = 1f)
    {
        if (Mathf.Abs(direction.y) == 0 || !canFlip)
        {
            return;
        }

        if (_simon != null && !_simon.allowInput)
        {
            return;
        }

        if (RotateLeverCoroutine != null)
        {
            StopCoroutine(RotateLeverCoroutine);
        }
        RotateLeverCoroutine = StartCoroutine(StartRotate());

        canFlip = false;
        OnLeverFlipped.Invoke(direction.y > 0 ? LeverUp : LeverDown);

        IEnumerator StartRotate()
        {
            duration = duration / 2;

            var timer = 0f;
            while (timer < duration)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                var t = timer / duration;

                var desiredEuler = defaultGbRotation.eulerAngles;
                desiredEuler.z += 45f * direction.normalized.y;

                var desiredRotation = Quaternion.Euler(desiredEuler);

                _pivot.transform.rotation = Quaternion.Lerp(_pivot.transform.rotation, desiredRotation, t);
                timer += Time.deltaTime;
            }

            canFlip = true;
            var returnTimer = 0f;
            while (returnTimer < duration)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                var t = returnTimer / duration;

                _pivot.transform.rotation = Quaternion.Lerp(_pivot.transform.rotation, defaultGbRotation, t);
                returnTimer += Time.deltaTime;
                
            }
        }
    }
}
