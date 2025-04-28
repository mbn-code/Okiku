using UnityEngine;

public class TekeTekeMoveFixed : MonoBehaviour
{
    public Camera targetCamera;
    public float moveSpeed = 5f;
    public float stopDistance = 1f;

    public AudioClip initialTekeTekeSound;
    public AudioClip pulseSoundCue;
    public AudioClip jumpscareSound;

    public float minPulseInterval = 0.05f;
    public float maxPulseInterval = 2f;
    public float minTekeTekeInterval = 0.1f;
    public float maxTekeTekeInterval = 1.0f;

    public float initialTekeTekeVolume = 0.3f;
    public float maxTekeTekeVolume = 1.0f;

    private bool hasPlayedJumpscareSound = false;
    private bool movementStarted = false;

    private Vector3 startingPosition;
    private float totalDistanceToCamera;
    private float pulseTimer = 0f;
    private float tekeTekeTimer = 0f;

    private AudioSource initialTekeTekeAudioSource;
    private AudioSource movingTekeTekeAudioSource;
    private AudioSource pulseAudioSource;
    private AudioSource jumpscareAudioSource;

    void Start()
    {
        startingPosition = transform.position;
        totalDistanceToCamera = Vector3.Distance(startingPosition, targetCamera.transform.position);

        // Set up audio sources
        initialTekeTekeAudioSource = gameObject.AddComponent<AudioSource>();
        initialTekeTekeAudioSource.spatialBlend = 0f;
        initialTekeTekeAudioSource.playOnAwake = false;

        movingTekeTekeAudioSource = gameObject.AddComponent<AudioSource>();
        movingTekeTekeAudioSource.spatialBlend = 0f;
        movingTekeTekeAudioSource.playOnAwake = false;

        pulseAudioSource = gameObject.AddComponent<AudioSource>();
        pulseAudioSource.spatialBlend = 0f;
        pulseAudioSource.playOnAwake = false;

        jumpscareAudioSource = gameObject.AddComponent<AudioSource>();
        jumpscareAudioSource.spatialBlend = 0f;
        jumpscareAudioSource.playOnAwake = false;

        PlayInitialTekeTekeSound();
    }

    void Update()
    {
        if (targetCamera == null)
            return;

        if (!movementStarted)
            return;

        float distanceToCamera = Vector3.Distance(transform.position, targetCamera.transform.position);

        if (distanceToCamera > stopDistance)
        {
            Vector3 direction = (targetCamera.transform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        float traveledDistance = totalDistanceToCamera - distanceToCamera;
        float distanceFactor = Mathf.Clamp01(traveledDistance / totalDistanceToCamera);

        // Adjust moving Teke Teke volume dynamically
        movingTekeTekeAudioSource.volume = Mathf.Lerp(initialTekeTekeVolume, maxTekeTekeVolume, distanceFactor);

        // Pulse logic
        float currentPulseInterval = Mathf.Lerp(maxPulseInterval, minPulseInterval, distanceFactor);
        pulseTimer += Time.deltaTime;

        if (pulseTimer >= currentPulseInterval)
        {
            PlayPulseSound();
            pulseTimer = 0f;
        }

        // Moving TekeTeke retrigger logic
        float currentTekeTekeInterval = Mathf.Lerp(maxTekeTekeInterval, minTekeTekeInterval, distanceFactor);
        tekeTekeTimer += Time.deltaTime;

        if (tekeTekeTimer >= currentTekeTekeInterval)
        {
            PlayMovingTekeTekeSound();
            tekeTekeTimer = 0f;
        }

        // Jumpscare halfway
        if (!hasPlayedJumpscareSound && traveledDistance >= totalDistanceToCamera / 2f)
        {
            PlayJumpscareSound();
            hasPlayedJumpscareSound = true;
        }
    }

    void PlayInitialTekeTekeSound()
    {
        if (initialTekeTekeSound != null)
        {
            initialTekeTekeAudioSource.clip = initialTekeTekeSound;
            initialTekeTekeAudioSource.volume = initialTekeTekeVolume;
            initialTekeTekeAudioSource.loop = false;
            initialTekeTekeAudioSource.Play();

            Invoke(nameof(StartMovementAfterInitialSound), initialTekeTekeSound.length);
        }
        else
        {
            StartMovementAfterInitialSound();
        }
    }

    void StartMovementAfterInitialSound()
    {
        movementStarted = true;
    }

    void PlayPulseSound()
    {
        if (pulseSoundCue != null)
        {
            pulseAudioSource.PlayOneShot(pulseSoundCue);
        }
    }

    void PlayJumpscareSound()
    {
        if (jumpscareSound != null)
        {
            jumpscareAudioSource.PlayOneShot(jumpscareSound);
        }
    }

    void PlayMovingTekeTekeSound()
    {
        if (initialTekeTekeSound != null)
        {
            movingTekeTekeAudioSource.PlayOneShot(initialTekeTekeSound);
        }
    }
}
