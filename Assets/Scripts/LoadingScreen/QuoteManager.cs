using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuoteManager : MonoBehaviour
{
    public List<string> Quotes;
    public TextMeshProUGUI textObject;

    public float fadeDuration = 0.25f;
    public float quoteDisplayTime = 5f;

    void Start()
    {
        StartCoroutine(SwitchQuote());
    }

    IEnumerator SwitchQuote()
    {
        while (true)
        {
            if (Quotes.Count == 0)
                yield break;

            string randomQuote = Quotes[Random.Range(0, Quotes.Count)];
            string[] quoteInfo = randomQuote.Split("|||");
            string newText = quoteInfo[0] + "\n" + quoteInfo[1];

            // Fade out current text
            yield return StartCoroutine(FadeText(0f));

            // Change the quote text
            textObject.text = newText;

            // Fade in new text
            yield return StartCoroutine(FadeText(1f));

            // Wait before next transition
            yield return new WaitForSeconds(quoteDisplayTime);
        }
    }

    IEnumerator FadeText(float targetAlpha)
    {
        Color currentColor = textObject.color;
        float startAlpha = currentColor.a;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            textObject.color = new Color(currentColor.r, currentColor.g, currentColor.b, newAlpha);
            yield return null;
        }

        // Ensure final alpha is set
        textObject.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}
