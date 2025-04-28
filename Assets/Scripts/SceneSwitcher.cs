using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public Image FadeoutImg;
    public float FadeDuration;

    public static int TargetScene; // Store which scene we want to load

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void SwitchScene(int sceneId)
    {
        TargetScene = sceneId;
        StartCoroutine(FadeAndChangeScene());
    }

    private IEnumerator FadeIn()
    {
        if (FadeoutImg != null)
        {
            FadeoutImg.gameObject.SetActive(true);

            Color color = FadeoutImg.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, time / FadeDuration);
                FadeoutImg.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 0f;
            FadeoutImg.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
    }

    private IEnumerator FadeAndChangeScene()
    {
        if (FadeoutImg != null)
        {
            FadeoutImg.gameObject.SetActive(true);

            Color color = FadeoutImg.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, time / FadeDuration);
                FadeoutImg.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            FadeoutImg.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
        SceneManager.LoadScene(6);
    }
}