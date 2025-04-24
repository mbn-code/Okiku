using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text buttonText;
    public float targetFontSize = 76f;
    public float animationDuration = 0.1f;

    private float originalFontSize;
    private Coroutine currentCoroutine;

    void Start()
    {
        if (buttonText == null)
        {
            buttonText = GetComponentInChildren<TMP_Text>();
        }

        if (buttonText != null)
        {
            originalFontSize = buttonText.fontSize;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(AnimateFontSize(buttonText.fontSize, targetFontSize));
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (buttonText != null)
        {
            if (currentCoroutine != null) StopCoroutine(currentCoroutine);
            currentCoroutine = StartCoroutine(AnimateFontSize(buttonText.fontSize, originalFontSize));
        }
    }

    private IEnumerator AnimateFontSize(float startSize, float endSize)
    {
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float newSize = Mathf.Lerp(startSize, endSize, elapsed / animationDuration);
            buttonText.fontSize = newSize;
            yield return null;
        }

        buttonText.fontSize = endSize;
    }
}
