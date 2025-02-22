using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class NoiseController : MonoBehaviour
{
    [Header("Sneak meter info")]
    public float noiseThreshold = 1f;
    public float currentNoise = 0f;
    public float noiseDegradation = .03f;
    public bool isDegrading = false;

    // Coroutines
    private Coroutine degradeCoroutine;

    // Event
    public UnityEvent<float> onNoiseChange;
    public UnityEvent onThreshold;

    
    void Update()
    {
        if (isDegrading && currentNoise > 0f)
        {
            currentNoise -=noiseDegradation * Time.deltaTime;
            // For when updating Noise Meter in UI
            onNoiseChange.Invoke(currentNoise/noiseThreshold);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            AddNoise(0.3f);
        }
    }

    public void AddNoise(float amount)
    {
        currentNoise = Mathf.Clamp(currentNoise + amount, 0, noiseThreshold);
        onNoiseChange.Invoke(currentNoise / noiseThreshold);

        // Call a method when noise threshold is reached (GameOver || CallHouseOwner)
        if (currentNoise >= noiseThreshold)
        {
            onThreshold.Invoke();
        }

        if (degradeCoroutine != null)
        {
            StopCoroutine(degradeCoroutine);
        }
        degradeCoroutine = StartCoroutine(DegradeCoroutine());

        // Wait for sometime before degrading noise
        IEnumerator DegradeCoroutine() {
            isDegrading = false;
            yield return new WaitForSeconds(1.5f);
            isDegrading = true;
        }
    }
}
