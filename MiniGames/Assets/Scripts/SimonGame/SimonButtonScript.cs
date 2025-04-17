using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class SimonButtonScript : MonoBehaviour
{
    [SerializeField] public string buttonName = null;
    [SerializeField] private SimonSaysScript _simonGame;
    private SpriteRenderer _sprite;

    public bool mouseHover;
    public bool mousePressed;

    private string defaultColour = "#434343";
    private string hoverColour = "#8C8E23";
    private string blinkColour = "#FFFFFF";
    private Vector3 defaultScale;

    public UnityEvent<string> OnButtonPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        buttonName = gameObject.name;
        SetSpriteColour(defaultColour);

        _simonGame = GetComponentInParent<SimonSaysScript>();
        OnButtonPressed.AddListener(_simonGame.OnButtonPressed);
        _simonGame.availableInputs = _simonGame.availableInputs.Append(buttonName).ToArray();

        defaultScale = transform.localScale;
    }

    private void OnMouseEnter()
    {
        if (mouseHover || !_simonGame.allowInput)
        {
            return;
        }

        mouseHover = true;
        SetHoverEffect();
    }

    private void OnMouseExit()
    {
        if (!mouseHover)
        {
            return;
        }

        mouseHover = false;
        SetHoverEffect();
    }

    private void OnMouseDown()
    {
        if (!_simonGame.allowInput)
        {
            return;
        }

        mousePressed = true;
        SetPressedEffect();

        OnButtonPressed.Invoke(buttonName);
    }

    private void OnMouseUp()
    {
        if (!mousePressed)
        {
            return ;
        }

        SetHoverEffect();
    }

    private void SetHoverEffect()
    {
        string colourString = mouseHover ? hoverColour : defaultColour;
        var scale = mouseHover ? 1.05f : 1f;

        SetSpriteColour(colourString);

        transform.localScale = scale * defaultScale;
    }

    private void SetPressedEffect()
    {
        var scale = mousePressed ? 0.95f : 1;
        string colourString = mousePressed ? blinkColour : defaultColour;

        transform.localScale = scale * defaultScale;
        SetSpriteColour(colourString);
    }

    private Coroutine BlinkCouroutine;
    public void Blink(float duration)
    {
        if (BlinkCouroutine != null)
        {
            StopCoroutine(BlinkCouroutine);
        }

        BlinkCouroutine = StartCoroutine(StartBlinkCoroutine());

        IEnumerator StartBlinkCoroutine()
        {
            SetSpriteColour(blinkColour);
            yield return new WaitForSeconds(duration);
            SetSpriteColour(defaultColour);
        }
    }

    private void SetSpriteColour(string colourString)
    {
        Color colourDefault;
        if (ColorUtility.TryParseHtmlString(colourString, out colourDefault))
        {
            _sprite.color = colourDefault;
        }
    }
}
