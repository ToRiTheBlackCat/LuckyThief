using UnityEngine;

[SelectionBase]
public class ThrowableScript : MonoBehaviour
{
    public Sprite objectSprite;
    [SerializeField] private SpriteRenderer BallRenderer;

    public float initialSpeed;
    public float throwAngleDegrees;
    const float gravity = 9.8f;
    public float time = 0.0f;

    public Vector2 initialPosition;
    public Vector2 throwDirection;

    public float zAxis = 0f;
    public bool isLaunch = false;

    
    private void Start()
    {
        if (objectSprite != null)
        {
            BallRenderer.sprite = objectSprite; 
        }
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;

        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    LaunchProjectile(transform.position, new Vector2(1, 0), 10f, 75f);
        //}

        if (isLaunch)
        {
            zAxis = initialSpeed * Mathf.Sin(throwAngleDegrees * Mathf.Deg2Rad) * time - 0.5f * gravity * Mathf.Pow(time, 2f);


            if (zAxis > 0f)
            {
                var xAxis = initialSpeed * Mathf.Cos(throwAngleDegrees * Mathf.Deg2Rad) * time;
                transform.position = initialPosition + throwDirection * xAxis;

                BallRenderer.gameObject.transform.localPosition = new Vector3(0, zAxis, 0);
                BallRenderer.transform.Rotate(new Vector3(0, 0, 45));
            }
            else
            {
                isLaunch = false;
                BallRenderer.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    public void LaunchProjectile(Vector2 initialPos, Vector2 lastPos, float angle)
    {
        initialPosition = initialPos;
        throwDirection = (lastPos - initialPos).normalized;

        var distance = (lastPos - initialPos).magnitude;

        throwAngleDegrees = angle;

        initialSpeed = Mathf.Pow(
                (distance * gravity / Mathf.Sin(2 * angle * Mathf.Deg2Rad)),
                0.5f
            );

        transform.position = initialPosition;
        time = 0.0f;
        isLaunch = true;
    }
}
