using UnityEngine;
using UnityEngine.UI;

public class InspectableObject : MonoBehaviour
{
    public Image inspectImage;           // Assign in inspector (shown when inspecting)
    public GameObject keyInfoUI;         // Assign in inspector (e.g., "Press E to Inspect")
    public float inspectDistance = 3f;

    private bool isInspecting = false;
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;

        if (inspectImage != null)
            inspectImage.gameObject.SetActive(false);

        if (keyInfoUI != null)
            keyInfoUI.SetActive(false);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        if (!isInspecting)
        {
            if (distance <= inspectDistance)
            {
                if (keyInfoUI != null)
                    keyInfoUI.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    StartInspecting();
                }
            }
            else
            {
                if (keyInfoUI != null)
                    keyInfoUI.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopInspecting();
            }
        }
    }

    void StartInspecting()
    {
        isInspecting = true;

        if (inspectImage != null)
        {
            inspectImage.gameObject.SetActive(true);
            ScaleImageToViewport();
        }

        if (keyInfoUI != null)
            keyInfoUI.SetActive(false);

        Time.timeScale = 0f; // Optional pause
    }

    void StopInspecting()
    {
        isInspecting = false;

        if (inspectImage != null)
            inspectImage.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void ScaleImageToViewport()
    {
        if (inspectImage != null && inspectImage.sprite != null)
        {
            RectTransform rt = inspectImage.rectTransform;

            float screenHeight = Screen.height;
            float targetHeight = screenHeight * 0.8f;

            float aspectRatio = inspectImage.sprite.bounds.size.x / inspectImage.sprite.bounds.size.y;
            float targetWidth = targetHeight * aspectRatio;

            rt.sizeDelta = new Vector2(targetWidth, targetHeight);
            rt.anchoredPosition = Vector2.zero; // Center the image
        }
    }
}
