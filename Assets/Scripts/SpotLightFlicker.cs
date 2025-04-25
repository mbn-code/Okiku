using UnityEngine;

[RequireComponent(typeof(Light))]
[DisallowMultipleComponent]
public class SpotlightFlicker : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("Minimum light intensity during flicker.")]
    public float minIntensity = 0.5f;

    [Tooltip("Maximum light intensity during flicker.")]
    public float maxIntensity = 1.5f;

    [Tooltip("Average time between flickers (affects both normal and burst mode).")]
    public float flickerSpeed = 0.1f;

    [Header("Burst Mode")]
    [Tooltip("Enable burst mode flickering.")]
    public bool useBurstMode = false;

    [Tooltip("Number of quick flickers per burst.")]
    public int burstCount = 4;

    [Tooltip("Pause time between bursts.")]
    public float burstInterval = 1.5f;

    private Light spotLight;
    private float timer;
    private int currentBurst = 0;
    private bool inBurst = false;

    void Awake()
    {
        spotLight = GetComponent<Light>();

        if (spotLight.type != LightType.Spot)
        {
            Debug.LogWarning("SpotlightFlicker is designed for Spot Lights only.", this);
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (useBurstMode)
        {
            HandleBurstMode();
        }
        else
        {
            HandleNormalFlicker();
        }
    }

    void HandleNormalFlicker()
    {
        if (timer <= 0f)
        {
            spotLight.intensity = Random.Range(minIntensity, maxIntensity);
            timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
        }
    }

    void HandleBurstMode()
    {
        if (inBurst)
        {
            if (timer <= 0f)
            {
                spotLight.intensity = Random.Range(minIntensity, maxIntensity);
                currentBurst++;

                if (currentBurst >= burstCount)
                {
                    inBurst = false;
                    timer = burstInterval;
                }
                else
                {
                    timer = Random.Range(flickerSpeed * 0.5f, flickerSpeed * 1.5f);
                }
            }
        }
        else
        {
            if (timer <= 0f)
            {
                inBurst = true;
                currentBurst = 0;
            }
        }
    }
}
