using UnityEngine;
using UnityEngine.SceneManagement; // Needed for loading scenes

public class DoorInteraction : MonoBehaviour
{
    public Transform player;
    public float interactionDistance = 3f;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionUI; // "Press E" UI (optional)
    public string sceneToLoad; // Name of the scene to load

    private bool isPlayerInRange = false;

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
}
