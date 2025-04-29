using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public List<string> CharacterNames;
    public List<Sprite> CharacterSprites;
    public List<string> Dialogues;

    [Range(0.1f, 1f)]
    public float textAnimationSpeed = 0.5f;

    public float DialogDistance = 2f;

    public GameObject MainCharacter;
    public bool IsTrigger;

    // Audio integration
    public AudioSource audioSource;                    // Assign in Inspector (or GetComponent<AudioSource> in Awake)
    public List<AudioClip> DialogueSounds;             // One clip per dialogue line (optional fallback if less clips than lines)

    private TMP_Text NameBox;
    private TMP_Text MessageBox;
    private GameObject dialogueWindow;
    private GameObject interactionUI;
    private Image DialogImage;
    private bool typing;
    private string currentMessage;
    private float startDialogueDelayTimer;
    private bool continuePressed = false;
    private bool skipAnim = false;
    private bool inDialog = false;
    private bool hasShown = false;
    private MainCharacter_Movement MainMovement;

    private void Awake()
    {
        NameBox = GameObject.Find("Dialog_Name").GetComponent<TMP_Text>();
        MessageBox = GameObject.Find("Dialog_Message").GetComponent<TMP_Text>();
        DialogImage = GameObject.Find("Dialog_Portait").GetComponent<Image>();
        dialogueWindow = GameObject.Find("Dialog_Box");
        interactionUI = GameObject.Find("Interaction_Box");

        MainMovement = MainCharacter.GetComponent<MainCharacter_Movement>();

        dialogueWindow.SetActive(false);
        interactionUI.SetActive(false);

        // Optionally grab AudioSource if not set in Inspector
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && inDialog)
        {
            if (typing)
            {
                skipAnim = true;
            }
            else
            {
                continuePressed = true;

                // Stop the current typing sound immediately
                if (audioSource != null)
                    audioSource.Stop();
            }
        }

        Vector3 MyPos = transform.position;
        Vector3 MainCharacterPos = MainCharacter.transform.position;

        if (Vector3.Distance(MainCharacterPos, MyPos) < DialogDistance && !inDialog)
        {
            if (IsTrigger && !hasShown)
            {
                StartCoroutine(StartDialogue()); // Force Start it
            }
            else if (!IsTrigger && !hasShown)
            {
                if (!interactionUI.activeSelf)
                    interactionUI.SetActive(true);

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    StartCoroutine(StartDialogue());
                }
            }
        }
        else
        {
            if (interactionUI.activeSelf)
                interactionUI.SetActive(false);
        }
    }

    IEnumerator StartDialogue()
    {
        hasShown = true;
        inDialog = true;
        MainMovement.SetDialog(true);
        dialogueWindow.SetActive(true);
        interactionUI.SetActive(false);

        for (int i = 0; i < Dialogues.Count; i++)
        {
            NameBox.text = CharacterNames[i % CharacterNames.Count];
            DialogImage.sprite = CharacterSprites[i % CharacterSprites.Count];

            // Play the corresponding dialogue sound (looped)
            if (audioSource != null)
            {
                if (DialogueSounds != null && i < DialogueSounds.Count && DialogueSounds[i] != null)
                {
                    audioSource.clip = DialogueSounds[i];
                    audioSource.Play();
                }
                else
                {
                    // No sound for this line
                    audioSource.Stop();
                }
            }

            yield return StartCoroutine(WriteTextToTextmesh(Dialogues[i], MessageBox)); // Wait for text animation

            yield return StartCoroutine(WaitForContinue());

            // Stop sound when line complete
            if (audioSource != null)
                audioSource.Stop();

            continuePressed = false;
        }

        // Ensure audio is off at the end
        if (audioSource != null)
            audioSource.Stop();

        dialogueWindow.SetActive(false);
        inDialog = false;
        MainMovement.SetDialog(false);
    }

    IEnumerator WaitForContinue()
    {
        while (!continuePressed)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WriteTextToTextmesh(string _text, TMP_Text _textMeshObject)
    {
        typing = true;

        _textMeshObject.text = string.Empty;
        char[] _letters = _text.ToCharArray();

        float _speed = 1f - textAnimationSpeed;

        foreach (char _letter in _letters)
        {
            if (skipAnim)
            {
                typing = false; // Stop animation immediately
                break;
            }

            _textMeshObject.text += _letter;

            if (_textMeshObject.text.Length == _letters.Length)
            {
                typing = false;
            }

            yield return new WaitForSeconds(0.1f * _speed);
        }

        // If skipped, finish text instantly
        if (skipAnim)
        {
            _textMeshObject.text = _text;
            skipAnim = false;
        }

        typing = false;
    }
}
