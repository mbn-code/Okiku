using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareManager : MonoBehaviour
{
    [Tooltip("The build index of the jumpscare scene (must be in Build Settings).")]
    public int sceneId = 3;

    public void TriggerJumpscare()
    {
        SceneManager.LoadScene(sceneId);
    }
}
