using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DoorPuzzle : MonoBehaviour
{
    public string correctPassword = "am i beautiful?";
    public int sceneBuildIndexToLoad = 4;
    public Transform player;
    public float interactionDistance = 2f;

    public GameObject keyInfoUI;
    public GameObject inputUI;
    public TMP_InputField passwordBox;
    public MainCharacter_Movement MainCharMov;
    public SceneSwitcher sceneSwitcher;

    private bool isPromptVisible = false;

    void Start()
    {
        if(inputUI != null)
        {
            inputUI.SetActive(false); // Ensure it's hidden initially
        }

        if (keyInfoUI != null)
        {
            keyInfoUI.SetActive(false); // Ensure it's hidden initially
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (keyInfoUI != null)
        {
            keyInfoUI.SetActive(distance <= interactionDistance);
        }

        if (distance <= interactionDistance)
        {
            if(Input.GetKeyDown(KeyCode.E) && !passwordBox.isFocused)
            {
                if (isPromptVisible)
                {
                    MainCharMov.SetDialog(false);
                    inputUI.SetActive(false);
                    isPromptVisible = false;
                }
                else
                {
                    MainCharMov.SetDialog(true);
                    inputUI.SetActive(true);
                    passwordBox.text = "";
                    isPromptVisible = true;
                }
            }
        }
        else
        {
            inputUI.SetActive(false);
            isPromptVisible = false;
        }
    }

    public void SubmitPassword()
    {
        if (passwordBox.text.ToLower() == correctPassword.ToLower())
        {
            inputUI.SetActive(false);
            sceneSwitcher.SwitchScene(sceneBuildIndexToLoad);
        }
        else
        {
            passwordBox.text = "Wrong password!";
        }
    }
}
