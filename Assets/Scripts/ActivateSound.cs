using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DelayedSoundPlayer : MonoBehaviour
{
    public float delay = 3f; // Time in seconds before the sound plays
    public AudioClip soundClip; // Assign your sound clip in the Inspector
    public float volume = 1f; // Volume of the sound

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (soundClip != null)
        {
            audioSource.clip = soundClip;
            StartCoroutine(PlaySoundAfterDelay());
        }
        else
        {
            Debug.LogWarning("No sound clip assigned to DelayedSoundPlayer.");
        }
    }

    System.Collections.IEnumerator PlaySoundAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        audioSource.volume = volume;
        audioSource.Play();
        Debug.Log("Sound played after " + delay + " seconds.");
    }
}
