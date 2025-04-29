using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class DoorPuzzle : MonoBehaviour
{
    public string correctPassword = "OpenSesame";
    public string sceneToLoad = "NextScene";

    private UIDocument uiDocument;
    private VisualElement root;
    private TextField passwordInput;
    private Label feedbackText;
    private Button submitButton;

    private bool playerInRange = false;

    void Start()
    {
        uiDocument = FindObjectOfType<UIDocument>();
        root = uiDocument.rootVisualElement;
        passwordInput = root.Q<TextField>("PasswordInput");
        feedbackText = root.Q<Label>("FeedbackText");
        submitButton = root.Q<Button>("SubmitButton");

        root.style.display = DisplayStyle.None;

        submitButton.clicked += SubmitPassword;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            root.style.display = DisplayStyle.Flex;
            passwordInput.value = "";
            feedbackText.text = "";
        }
    }

    void SubmitPassword()
    {
        if (passwordInput.value == correctPassword)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            feedbackText.text = "Wrong password!";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            root.style.display = DisplayStyle.None;
        }
    }
}
