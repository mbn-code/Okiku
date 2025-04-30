using UnityEngine;

public class FreezeOnSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource;          // The AudioSource to monitor
    public AudioClip freezeClip;             // The clip that triggers freezing

    [Header("Target to Freeze")]
    public MonoBehaviour targetScript;       // Script to disable (e.g., movement or AI)

    private void Update()
    {
        if (audioSource == null || freezeClip == null || targetScript == null)
            return;

        bool isFreezeClipPlaying = audioSource.isPlaying && audioSource.clip == freezeClip;

        // Only update if state changes
        if (targetScript.enabled == isFreezeClipPlaying)
        {
            targetScript.enabled = !isFreezeClipPlaying;
        }
    }
}
