using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{
    public Image FadeInImage;
    public float FadeDuration;

    private void Start()
    {
        StartCoroutine(LoadSceneAsync(SceneSwitcher.TargetScene));
    }

    private IEnumerator LoadSceneAsync(int sceneId)
    {
        yield return FadeIn(); // Fade in from blackscreen

        // Start loading the scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);
        asyncLoad.allowSceneActivation = false;

        // Optionally, wait until scene is mostly loaded
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(5f); // Wait a little while

        // Fade out or wait a bit
        yield return FadeOut();

        // Allow activation
        asyncLoad.allowSceneActivation = true;
    }

    private IEnumerator FadeIn()
    {
        if (FadeInImage != null)
        {
            FadeInImage.gameObject.SetActive(true);

            Color color = FadeInImage.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, time / FadeDuration);
                FadeInImage.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 0f;
            FadeInImage.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
    }

    private IEnumerator FadeOut()
    {
        if (FadeInImage != null)
        {
            FadeInImage.gameObject.SetActive(true);

            Color color = FadeInImage.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, time / FadeDuration);
                FadeInImage.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            FadeInImage.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
    }
}
