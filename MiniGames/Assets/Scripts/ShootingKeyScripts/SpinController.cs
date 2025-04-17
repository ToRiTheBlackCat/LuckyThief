using System.Collections;
using UnityEngine;

public class SpinController : MonoBehaviour
{
    public float rotationSpeed = 1f;
    private bool isRight = true;

    public int minSpeed;
    public int maxSpeed;

    private ShootingKeyAudioController audioController;

    private void Awake()
    {
        audioController = FindAnyObjectByType<ShootingKeyAudioController>();
    }

    void Start()
    {
        StartCoroutine(CheckDirectionChange()); // Start checking direction every 5s
    }

    void Update()
    {
        Spin();
    }

    void Spin()
    {
        transform.Rotate((isRight ? 1 : -1) * Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    IEnumerator CheckDirectionChange()
    {
        while (true)
        {

            yield return new WaitForSeconds(5f);
            rotationSpeed = RandomRotationSpeed();
            isRight = RandomLeftRight();
        }
    }
    float RandomRotationSpeed()
    {
        var r =  Random.Range(minSpeed, maxSpeed);
        if(r > 150)
            rotationSpeed = r;
       return rotationSpeed;
    }

    bool RandomLeftRight()
    {
        return Random.Range(0, 1) > 0.5f;
    }
}
