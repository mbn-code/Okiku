using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProximitySoundTrigger : MonoBehaviour
{
    [Tooltip("The target GameObject to detect (e.g. the player).")]
    public Transform target;

    [Tooltip("Trigger range in units.")]
    public float triggerDistance = 5f;

    [Tooltip("Should the sound only play once?")]
    public bool playOnce = true;

    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (target == null)
        {
            Debug.LogWarning("No target assigned for ProximitySoundTrigger.", this);
        }
    }

    void Update()
    {
        if (target == null || audioSource == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= triggerDistance)
        {
            if (!audioSource.isPlaying && (!playOnce || !hasPlayed))
            {
                audioSource.Play();
                hasPlayed = true;
            }
        }
    }
}
