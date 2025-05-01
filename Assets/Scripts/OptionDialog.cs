using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionDialog : MonoBehaviour
{
    public string FirstSentece = "Am I beautiful?";
    public string FirstCharacter = "Kochisake";
    public Sprite FirstCharacterImage;
    public AudioClip FirstAudioClip;

    // Yes or no

    public string SecondSentence = "What about now?";
    public string SecondCharacter = "Kochisake";
    public Sprite SecondCharacterImage;
    public AudioClip SecondAudioClip;

    public string ThirdSentence = "Am I beautiful?";
    public string ThirdCharacter = "Okiku";
    public Sprite ThirdCharacterImage;
    public AudioClip ThirdAudioClip;

    public List<string> YappingList;
    public string YapperName = "Kochisake";
    public Sprite YapperSprite;
    public List<AudioClip> YapperAudioClip;

    [Range(0.1f, 1f)]
    public float textAnimationSpeed = 0.5f;

    public float DialogDistance = 2f;

    public GameObject MainCharacter;
    public bool IsTrigger;

    // Audio integration
    public AudioSource audioSource;

    public TMP_Text NameBox;
    public TMP_Text MessageBox;
    public GameObject dialogueWindow;
    public GameObject interactionUI;
    public Image DialogImage;
    public GameObject OptionUI;
    private bool typing;
    private string currentMessage;
    private float startDialogueDelayTimer;
    private bool continuePressed = false;
    private bool skipAnim = false;
    private bool inDialog = false;
    private bool hasShown = false;
    public MainCharacter_Movement MainMovement;

    bool hasAnswered = false;
    bool answer = false;

    public JumpscareManager jmpScareMng;
    public SceneSwitcher sceneSwitcher;

    public GameObject KuchisakeIdle;
    public Vector3 IdlePosition;

    private void Awake()
    {
        dialogueWindow.SetActive(false);
        interactionUI.SetActive(false);
        OptionUI.SetActive(false);

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
        KuchisakeIdle.gameObject.transform.position = IdlePosition;

        NameBox.text = FirstCharacter;
        DialogImage.sprite = FirstCharacterImage;
        audioSource.clip = FirstAudioClip;
        audioSource.Play();
        yield return StartCoroutine(WriteTextToTextmesh(FirstSentece, MessageBox)); // Wait for text animation
        OptionUI.SetActive(true);
        yield return StartCoroutine(WaitForAnswer());
        if (audioSource != null)
            audioSource.Stop();

        continuePressed = false;

        OptionUI.SetActive(false); // Hide yes or no
        hasAnswered = false; // Reset

        NameBox.text = SecondCharacter;
        DialogImage.sprite = SecondCharacterImage;
        audioSource.clip = SecondAudioClip;
        audioSource.Play();
        yield return StartCoroutine(WriteTextToTextmesh(SecondSentence, MessageBox)); // Wait for text animation
        OptionUI.SetActive(true);
        yield return StartCoroutine(WaitForAnswer());
        if (audioSource != null)
            audioSource.Stop();

        continuePressed = false;

        OptionUI.SetActive(false); // Hide yes or no

        if(!answer)
        {
            jmpScareMng.TriggerJumpscare();
            yield return null;
        }

        NameBox.text = ThirdCharacter;
        DialogImage.sprite = ThirdCharacterImage;
        audioSource.clip = ThirdAudioClip;
        audioSource.Play();
        yield return StartCoroutine(WriteTextToTextmesh(ThirdSentence, MessageBox)); // Wait for text animation
        yield return StartCoroutine(WaitForContinue());
        if (audioSource != null)
            audioSource.Stop();

        continuePressed = false;

        for (int i = 0; i < YappingList.Count; i++)
        {
            NameBox.text = YapperName;
            DialogImage.sprite = YapperSprite;

            // Play the corresponding dialogue sound (looped)
            if (audioSource != null)
            {
                if (YapperAudioClip != null && i < YapperAudioClip.Count && YapperAudioClip[i] != null)
                {
                    audioSource.clip = YapperAudioClip[i];
                    audioSource.Play();
                }
                else
                {
                    // No sound for this line
                    audioSource.Stop();
                }
            }

            yield return StartCoroutine(WriteTextToTextmesh(YappingList[i], MessageBox)); // Wait for text animation

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

        sceneSwitcher.SwitchScene(13); // Post credits scene
    }

    IEnumerator WaitForContinue()
    {
        while (!continuePressed)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator WaitForAnswer()
    {
        while(!hasAnswered)
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

    public void Yes()
    {
        hasAnswered = true;
        answer = true;
    }

    public void No()
    {
        hasAnswered = true;
        answer = false;
    }
}
