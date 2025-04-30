using UnityEngine;
using UnityEngine.SceneManagement; // Needed for loading scenes
using TMPro; // Needed for TextMeshPro

public class DoorInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionUI; // Assign a UI Text or Panel (e.g., "Press E")
    public string InteractionText = "Press  ";
    private TMP_Text textBox;

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false); // Hide UI at start

        textBox = GameObject.Find("Input text").GetComponent<TMP_Text>();
    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        isPlayerInRange = distance <= interactionDistance;

        if (interactionUI != null)
            interactionUI.SetActive(isPlayerInRange);

        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    [Header("Scene to Load")]
    public int sceneToLoad;
}
