using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TekeTekeTeleporter : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 StartPositon;
    public Vector3 StartDirection;
    public float crawlSpeed = 10f; // speed when chasing
    public Transform mainCharacter;

    [Header("Trigger Settings")]
    public float requiredHeight  = 0.3f; // y > this = survive
    public float graceTime = 5f;

    public JumpscareManager jmpScareMng;
    public SceneSwitcher sceneSwitcher;

    private AudioSource warningAudio;

    private bool isAllowedToAttack = false;
    private bool shouldMove = false;

    void Start()
    {
        warningAudio = GetComponent<AudioSource>();
        if (warningAudio.clip == null)
            Debug.LogWarning($"{name}: AudioSource has no clip assigned!");

        // ensure the warning loops during its window
        warningAudio.loop = true;

        StartCoroutine(TeleporterRoutine());
    }

    private IEnumerator TeleporterRoutine()
    {
        // 1) Wait for being allowed to attack
        yield return WaitForAttack();

        while(isAllowedToAttack)
        {
            yield return new WaitForSeconds(graceTime);

            if (isAllowedToAttack)
            {

                if (!warningAudio.isPlaying)
                    warningAudio.Play();

                yield return new WaitForSeconds(0.5f);

                warningAudio.Stop();

                // Go in the direction
                transform.position = StartPositon;
                shouldMove = true;

                yield return new WaitForSeconds(6.5f); // Make sure it checks the entire map

                shouldMove = false;
            }
        }
    }

    private void Update()
    {
        if (shouldMove)
        {

            transform.position += StartDirection * crawlSpeed * Time.deltaTime;
        }
    }

    private IEnumerator WaitForAttack()
    {
        while (!isAllowedToAttack)
        {
            yield return new WaitForSeconds(0.1f);
        }

    }

    public void AllowAttack()
    {
        isAllowedToAttack = true;
    }

    public void DisallowAttack()
    {
        isAllowedToAttack = false;
        shouldMove = false;
        Debug.Log("No Longer Allowed");
    }
}
