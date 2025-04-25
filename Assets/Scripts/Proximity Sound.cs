using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class ProximityRepeatingSound : MonoBehaviour
{
    [Tooltip("The Transform (e.g. your Player) to measure distance to.")]
    public Transform target;

    [Tooltip("At this distance or beyond, volume == minVolume and sound won’t trigger.")]
    public float maxDistance = 20f;

    [Tooltip("Volume when right at maxDistance. 0 silences completely; 1 is full volume.")]
    [Range(0f, 1f)]
    public float minVolume = 0f;

    [Tooltip("How many times to play the clip, once per delay.")]
    public int repeatCount = 3;

    [Tooltip("Seconds between each play.")]
    public float repeatDelay = 1f;

    private AudioSource _audio;
    private bool _isInside = false;
    private Coroutine _playRoutine;

    void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.loop = false;            // ensure it doesn’t loop on its own
        if (_audio.clip == null)
            Debug.LogWarning($"{name}: No AudioClip assigned to the AudioSource!");
        if (target == null)
            Debug.LogWarning($"{name}: No target assigned—you’ll never hear this.");
    }

    void Update()
    {
        if (target == null || _audio.clip == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        // Compute 0..1 where 0 at 0m, 1 at maxDistance or beyond
        float t = Mathf.InverseLerp(0f, maxDistance, dist);
        // Volume = 1 at t=0, = minVolume at t=1
        float currVol = Mathf.Lerp(1f, minVolume, t);

        bool nowInside = dist <= maxDistance;

        // just-entered?
        if (nowInside && !_isInside)
        {
            _isInside = true;
            _playRoutine = StartCoroutine(PlayRepeatedly());
        }
        // just-exited?
        else if (!nowInside && _isInside)
        {
            _isInside = false;
            if (_playRoutine != null)
            {
                StopCoroutine(_playRoutine);
                _playRoutine = null;
            }
        }
    }

    private IEnumerator PlayRepeatedly()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            // recompute volume at the moment of each play
            float dist = Vector3.Distance(transform.position, target.position);
            float t = Mathf.InverseLerp(0f, maxDistance, dist);
            float vol = Mathf.Lerp(1f, minVolume, t);

            _audio.PlayOneShot(_audio.clip, vol);
            yield return new WaitForSeconds(repeatDelay);
        }
    }
}
