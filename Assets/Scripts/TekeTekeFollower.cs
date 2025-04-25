using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TekeTekeTeleporter : MonoBehaviour
{
    [Header("Player to Chase")]
    public Transform target;

    [Header("Speeds")]
    public float crawlSpeed    = 50f;  // fast chase
    public float slowSpeed     = 10f;  // creep during cooldown

    [Header("Timing (seconds)")]
    public float initialDelay  = 10f;  // before first warning
    public float warningTime   = 2f;   // how long the sound plays before each chase
    public float gracePeriod   = 5f;   // max time to climb or get caught
    public float cooldownPeriod= 5f;   // slow‐move after surviving

    [Header("Trigger Settings")]
    public float triggerDistance = 5f;   // when to snap‐check height
    public float requiredHeight  = 0.3f; // y > this = survive

    private AudioSource warningAudio;

    void Start()
    {
        warningAudio = GetComponent<AudioSource>();
        if (warningAudio.clip == null)
            Debug.LogWarning($"{name}: AudioSource has no clip assigned!");
        if (target == null)
            Debug.LogWarning($"{name}: No target set – nothing to chase.");

        // ensure the warning loops during its window
        warningAudio.loop = true;

        StartCoroutine(TeleporterRoutine());
    }

    private IEnumerator TeleporterRoutine()
    {
        // 1) Initial silent delay
        yield return new WaitForSeconds(initialDelay);

        // 2) Repeat until the player dies
        while (true)
        {
            // --- Warning window ---
            warningAudio.Play();
            yield return new WaitForSeconds(warningTime);
            warningAudio.Stop();

            // --- Fast chase + height check ---
            bool survived = false;
            float timer = 0f;

            while (true)
            {
                // move fast toward player
                if (target != null)
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target.position,
                        crawlSpeed * Time.deltaTime
                    );

                // if close enough, check height
                if (target != null &&
                    Vector3.Distance(transform.position, target.position) <= triggerDistance)
                {
                    survived = target.position.y > requiredHeight;
                    Debug.Log(survived ? "Player survived!" : "Player died!");
                    break;
                }

                // if grace time runs out, force check
                if (timer >= gracePeriod)
                {
                    survived = (target != null && target.position.y > requiredHeight);
                    Debug.Log(survived ? "Player survived!" : "Player died!");
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // ensure the warning is stopped
            warningAudio.Stop();

            // if the player died, exit the loop (and coroutine)
            if (!survived)
                yield break;

            // --- Cooldown slow‐move for a bit ---
            float cooldownTimer = 0f;
            while (cooldownTimer < cooldownPeriod)
            {
                if (target != null)
                    transform.position = Vector3.MoveTowards(
                        transform.position,
                        target.position,
                        slowSpeed * Time.deltaTime
                    );
                cooldownTimer += Time.deltaTime;
                yield return null;
            }

            // then loop back to warning → chase again
        }
    }
}
