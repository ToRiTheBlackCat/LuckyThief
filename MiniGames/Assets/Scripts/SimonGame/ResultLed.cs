using System.Collections;
using UnityEngine;

public class ResultLed : MonoBehaviour
{
    [SerializeField] private Sprite _rightSprite;
    [SerializeField] private Sprite _wrongSprite;
    [SerializeField] private Sprite _offSprite;
    private SpriteRenderer _renderer;

    private Coroutine BlinkCoroutine;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetResult(bool right)
    {
        if (BlinkCoroutine != null)
        {
            StopCoroutine(BlinkCoroutine);
        }
        BlinkCoroutine = StartCoroutine(Blink());

        IEnumerator Blink()
        {
            var sprite = right ? _rightSprite : _wrongSprite;

            _renderer.sprite = sprite;
            yield return new WaitForSecondsRealtime(.15f);

            var blinkCounter = 0;
            while (blinkCounter <= 3)
            {
                _renderer.sprite = _offSprite;
                yield return new WaitForSecondsRealtime(.15f);
                _renderer.sprite = sprite;
                yield return new WaitForSecondsRealtime(.15f);

                blinkCounter++;
            }
        }
    }

    public void TurnOff()
    {
        if (BlinkCoroutine != null)
        {
            StopCoroutine(BlinkCoroutine);
        }
        _renderer.sprite = _offSprite;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.V))
    //    {
    //        SetResult(true);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.B))
    //    {
    //        SetResult(false);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.C))
    //    {
    //        TurnOff();
    //    }
    //}
}
