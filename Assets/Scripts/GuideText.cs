using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideText : MonoBehaviour
{
    public List<string> GuideMessages;
    public TMP_Text GuideBox;
    public float MessageTime = 5f;
    public float fadeDuration = 0.25f;
    public float BetweenTime = 5f;

    // Start is called before the first frame update
    void Start()
    {
        Color currentColor = GuideBox.color;
        GuideBox.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0); // Hide default
        StartCoroutine(showMessages());
    }

    IEnumerator showMessages()
    {
        if (GuideMessages.Count == 0)
            yield return null;

        GuideBox.text = GuideMessages[0];

        yield return StartCoroutine(FadeText(1f));

        yield return new WaitForSeconds(MessageTime);

        for (int i = 0; i < GuideMessages.Count; i++)
        {
            if (i == 0)
                continue;

            // Fade out current text
            yield return StartCoroutine(FadeText(0f));

            // Change the quote text
            GuideBox.text = GuideMessages[i];

            yield return new WaitForSeconds(BetweenTime);

            // Fade in new text
            yield return StartCoroutine(FadeText(1f));

            yield return new WaitForSeconds(MessageTime);
        }

        // Remove the last text
        yield return StartCoroutine(FadeText(0f));
    }

    IEnumerator FadeText(float targetAlpha)
    {
        Color currentColor = GuideBox.color;
        float startAlpha = currentColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            GuideBox.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // Ensure final alpha is set
        GuideBox.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}
