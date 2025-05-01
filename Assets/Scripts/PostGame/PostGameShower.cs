using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PostGameShower : MonoBehaviour
{
    public TMP_Text thanksText;
    public TMP_Text realText;
    public Image realImage;
    public float FadeDuration = 1f;
    public SceneSwitcher sceneSwitcher;

    private void Start()
    {
        thanksText.gameObject.SetActive(true);
        realText.gameObject.SetActive(false);
        realImage.gameObject.SetActive(false);

        StartCoroutine(ShowInfo());
    }

    private IEnumerator ShowInfo()
    {
        yield return new WaitForSeconds(5f);

        yield return FadeIn(thanksText);

        yield return FadeOut(realText);
        yield return FadeOut(realImage);

        yield return new WaitForSeconds(5f);

        yield return FadeIn(realText);
        yield return FadeIn(realImage);

        yield return new WaitForSeconds(2.5f);

        sceneSwitcher.SwitchFastScene(0); // Switch to the main menu scene
    }

    private IEnumerator FadeOut(TMP_Text text)
    {
        if (text != null)
        {
            text.gameObject.SetActive(true);

            Color color = text.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, time / FadeDuration);
                text.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            text.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
    }

    private IEnumerator FadeIn(TMP_Text text)
    {
        if (text != null)
        {
            text.gameObject.SetActive(true);

            Color color = text.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, time / FadeDuration);
                text.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 0f;
            text.color = color;

            text.gameObject.SetActive(false); // Hide the text after fading out
        }
    }

    private IEnumerator FadeOut(Image img)
    {
        if (img != null)
        {
            img.gameObject.SetActive(true);

            Color color = img.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(0f, 1f, time / FadeDuration);
                img.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 1f;
            img.color = color;
        }

        yield return new WaitForSeconds(0.3f); // small pause
    }

    private IEnumerator FadeIn(Image text)
    {
        if (text != null)
        {
            text.gameObject.SetActive(true);

            Color color = text.color;
            float time = 0f;

            while (time < FadeDuration)
            {
                color.a = Mathf.Lerp(1f, 0f, time / FadeDuration);
                text.color = color;
                time += Time.deltaTime;
                yield return null;
            }

            color.a = 0f;
            text.color = color;

            text.gameObject.SetActive(false); // Hide the text after fading out
        }
    }
}
