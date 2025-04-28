using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PositionTriggerSceneChange : MonoBehaviour
{
    [Header("Target Settings")]
    [Tooltip("The target Transform to monitor (e.g., the player).")]
    [SerializeField] private Transform target;

    [Tooltip("How close the target must get to trigger.")]
    [SerializeField] private float triggerRadius = 2f;

    [Header("Scene Settings")]
    [Tooltip("The name of the scene to load when triggered.")]
    [SerializeField] private string sceneToLoad;

    [Header("Fade Settings")]
    [Tooltip("UI Image used for screen fade effect.")]
    [SerializeField] private Image fadeImage;

    [Tooltip("Duration of the fade effect (in seconds).")]
    [SerializeField] private float fadeDuration = 1f;

    private bool triggered = false;

    private void Update()
    {
        if (triggered || target == null) return;

        // Now using THIS GameObject's position
        if (Vector3.Distance(target.position, transform.position) <= triggerRadius)
        {
            triggered = true;
            StartCoroutine(FadeAndChangeScene());
        }
    }

    private IEnumerator FadeAndChangeScene()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color color = fadeImage.color;
            float time = 0f;

            while (time < fadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, time / fadeDuration);
                fadeImage.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            fadeImage.color = color;
        }

        yield return new WaitForSeconds(0.2f); // optional small pause
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere in the Scene view to visualize the trigger radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
    }
}
