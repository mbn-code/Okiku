using UnityEngine;
using UnityEngine.SceneManagement; // Needed for loading scenes

public class DoorInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionUI; // Assign a UI Text or Panel (e.g., "Press E")

    private bool isPlayerInRange = false;

    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false); // Hide UI at start
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
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogWarning("No scene assigned to the DoorInteraction script!");
        }
    }

    [Header("Scene to Load")]
    public string sceneToLoad;
}
