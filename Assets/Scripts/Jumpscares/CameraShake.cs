using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1f;
    public float shakeMagnitude = 0.3f;
    public float shakeDecay = 1f; // How fast shake weakens
    public float shakeDelay = 0f;

    private Vector3 originalPosition;
    private bool isShaking = false;
    private float shakeTimeRemaining = 0f;

    void Start()
    {
        originalPosition = transform.localPosition;
        StartCoroutine(DelayShake());
    }

    IEnumerator DelayShake()
    {
        yield return new WaitForSeconds(shakeDelay);
        StartShake();
    }

    void Update()
    {
        if (isShaking)
        {
            if (shakeTimeRemaining > 0)
            {
                float currentMagnitude = shakeMagnitude * (shakeTimeRemaining / shakeDuration);

                transform.localPosition = originalPosition + Random.insideUnitSphere * currentMagnitude;

                shakeTimeRemaining -= Time.deltaTime * shakeDecay;
            }
            else
            {
                isShaking = false;
                shakeTimeRemaining = 0f;
                transform.localPosition = originalPosition;
            }
        }
    }

    void StartShake()
    {
        isShaking = true;
        shakeTimeRemaining = shakeDuration;
    }
}
