using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
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

    [Header("Audio Settings")]
    [Tooltip("Sound effect to play during flicker.")]
    public AudioClip flickerSound;

    [Tooltip("Minimum time between sound plays.")]
    public float minSoundInterval = 0.4f;

    [Tooltip("Maximum time between sound plays.")]
    public float maxSoundInterval = 0.6f;

    [Tooltip("Minimum pitch for the flicker sound.")]
    [Range(0.1f, 3f)]
    public float minPitch = 0.9f;

    [Tooltip("Maximum pitch for the flicker sound.")]
    [Range(0.1f, 3f)]
    public float maxPitch = 1.1f;

    [Tooltip("Minimum volume for the flicker sound.")]
    [Range(0f, 1f)]
    public float minVolume = 0.8f;

    [Tooltip("Maximum volume for the flicker sound.")]
    [Range(0f, 1f)]
    public float maxVolume = 1.0f;

    private Light spotLight;
    private AudioSource audioSource;
    private float timer;
    private float soundTimer;
    private int currentBurst = 0;
    private bool inBurst = false;

    void Awake()
    {
        spotLight = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();

        if (spotLight.type != LightType.Spot)
        {
            Debug.LogWarning("SpotlightFlicker is designed for Spot Lights only.", this);
        }

        soundTimer = Random.Range(minSoundInterval, maxSoundInterval);
    }

    void Update()
    {
        timer -= Time.deltaTime;
        soundTimer -= Time.deltaTime;

        if (useBurstMode)
        {
            HandleBurstMode();
        }
        else
        {
            HandleNormalFlicker();
        }

        HandleSoundPlayback();
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

    void HandleSoundPlayback()
    {
        if (soundTimer <= 0f && flickerSound != null && audioSource != null)
        {
            // Set random pitch and volume before playing
            audioSource.pitch = Random.Range(minPitch, maxPitch);
            audioSource.volume = Random.Range(minVolume, maxVolume);

            audioSource.PlayOneShot(flickerSound);
            soundTimer = Random.Range(minSoundInterval, maxSoundInterval);
        }
    }
}
