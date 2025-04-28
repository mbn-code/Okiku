using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySoundOnStart : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioSource found on " + gameObject.name);
        }
    }
}
