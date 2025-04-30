using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DoorPuzzle : MonoBehaviour
{
    public string correctPassword = "Am I beautiful?";
    public int sceneBuildIndexToLoad = 4;
    public Transform player;
    public float interactionDistance = 2f;

    public GameObject keyInfoUI; // 👈 assign the Keyinfo GameObject in Inspector

    private VisualElement root;
    private TextField passwordInput;
    private Button submitButton;
    private Label feedbackText;
    private VisualElement dialog;

    private bool isPromptVisible = false;

    void Start()
    {
        var uiDocument = FindObjectOfType<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found in scene!");
            return;
        }

        root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("rootVisualElement is null!");
            return;
        }

        // Build dialog UI in code...
        root.style.flexDirection = FlexDirection.Column;
        root.style.justifyContent = Justify.Center;
        root.style.alignItems = Align.Center;
        root.style.width = new Length(100, LengthUnit.Percent);
        root.style.height = new Length(100, LengthUnit.Percent);
        root.style.backgroundColor = new Color(0, 0, 0, 0.4f);

        // 📦 Dialog container
        dialog = new VisualElement();
        dialog.style.flexDirection = FlexDirection.Column;
        dialog.style.alignItems = Align.Center;
        dialog.style.width = 600;
        dialog.style.paddingTop = 40;
        dialog.style.paddingBottom = 40;
        dialog.style.paddingLeft = 50;
        dialog.style.paddingRight = 50;
        dialog.style.backgroundColor = new Color(0.15f, 0.15f, 0.15f, 1f);
        dialog.style.display = DisplayStyle.None;
        dialog.style.unityFontStyleAndWeight = FontStyle.Bold;
        root.Add(dialog);

        // 📝 Password input field
        passwordInput = new TextField("Enter the password:");
        passwordInput.style.fontSize = 32;
        passwordInput.style.width = new Length(100, LengthUnit.Percent);
        passwordInput.style.height = 60;
        passwordInput.labelElement.style.fontSize = 28;
        passwordInput.style.marginBottom = 25;
        dialog.Add(passwordInput);

        // 🔘 Submit button
        submitButton = new Button(() => SubmitPassword()) { text = "Submit" };
        submitButton.style.fontSize = 30;
        submitButton.style.width = new Length(100, LengthUnit.Percent);
        submitButton.style.height = 60;
        submitButton.style.marginBottom = 20;
        dialog.Add(submitButton);

        // ⚠️ Feedback text
        feedbackText = new Label("");
        feedbackText.style.color = Color.red;
        feedbackText.style.fontSize = 26;
        feedbackText.style.unityTextAlign = TextAnchor.MiddleCenter;
        dialog.Add(feedbackText);


        if (keyInfoUI != null)
        {
            keyInfoUI.SetActive(false); // Ensure it's hidden initially
        }
    }

    void Update()
    {
        if (root == null || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // 🔁 Show or hide Keyinfo GameObject based on distance
        if (keyInfoUI != null)
        {
            keyInfoUI.SetActive(distance <= interactionDistance);
        }

        if (distance <= interactionDistance)
        {
            if (Input.GetKeyDown(KeyCode.E) && !isPromptVisible)
            {
                dialog.style.display = DisplayStyle.Flex;
                passwordInput.value = "";
                feedbackText.text = "";
                isPromptVisible = true;
            }
        }
        else
        {
            dialog.style.display = DisplayStyle.None;
            isPromptVisible = false;
        }
    }

    void SubmitPassword()
    {
        if (passwordInput.value == correctPassword)
        {
            SceneManager.LoadScene(sceneBuildIndexToLoad);
        }
        else
        {
            feedbackText.text = "Wrong password!";
        }
    }
}
