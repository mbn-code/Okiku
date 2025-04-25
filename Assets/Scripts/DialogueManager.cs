using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
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

    private void Awake()
    {
        NameBox = GameObject.Find("Dialog_Name").GetComponent<TMP_Text>();
        MessageBox = GameObject.Find("Dialog_Message").GetComponent<TMP_Text>();
        DialogImage = GameObject.Find("Dialog_Portait").GetComponent<Image>();
        dialogueWindow = GameObject.Find("Dialog_Box");
        interactionUI = GameObject.Find("Interaction_Box");

        dialogueWindow.SetActive(false);
        interactionUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space) && inDialog)
        {
            if(typing)
            {
                skipAnim = true;
            } else
            {
                continuePressed = true;
            }
        }

        Vector3 MyPos = transform.position;
        Vector3 MainCharacterPos = MainCharacter.transform.position;

        if(Vector3.Distance(MainCharacterPos, MyPos) < DialogDistance && !inDialog)
        {
            if(IsTrigger && !hasShown)
            {
                StartCoroutine(StartDialogue()); // Force Start it
            } else if(!IsTrigger && !hasShown)
            {
                if (!interactionUI.activeSelf)
                    interactionUI.SetActive(true);

                if (Input.GetKeyUp(KeyCode.Space))
                {
                    StartCoroutine(StartDialogue());
                }
            }
        } else
        {
            if (interactionUI.activeSelf)
                interactionUI.SetActive(false);
        }
    }

    IEnumerator StartDialogue()
    {
        hasShown = true;
        inDialog = true;
        dialogueWindow.SetActive(true);
        interactionUI.SetActive(false);

        for (int i = 0; i < Dialogues.Count; i++)
        {
            NameBox.text = CharacterNames[i % CharacterNames.Count];
            DialogImage.sprite = CharacterSprites[i % CharacterSprites.Count];

            yield return StartCoroutine(WriteTextToTextmesh(Dialogues[i], MessageBox)); // Wait for it to finish

            yield return StartCoroutine(WaitForContinue());

            continuePressed = false;
        }

        dialogueWindow.SetActive(false);
        inDialog = false;
    }

    IEnumerator WaitForContinue()
    {
        while(!continuePressed)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WriteTextToTextmesh(string _text, TMP_Text _textMeshObject)
    {
        typing = true;

        _textMeshObject.text = "";
        char[] _letters = _text.ToCharArray();

        float _speed = 1f - textAnimationSpeed;

        foreach (char _letter in _letters)
        {
            if (skipAnim)
            {
                typing = false; // Stop animation
                break;
            }

            _textMeshObject.text += _letter;

            if (_textMeshObject.text.Length == _letters.Length)
            {
                typing = false;
            }

            yield return new WaitForSeconds(0.1f * _speed);
        }

        if(skipAnim)
        {
            _textMeshObject.text = _text;
            skipAnim = false;
        }
    }

}
