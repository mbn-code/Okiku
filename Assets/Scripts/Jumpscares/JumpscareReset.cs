using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpscareReset : MonoBehaviour
{
    public float DelayTime = 5f; // Time to wait before resetting the game
    public int SceneToLoad = 0; // Index of the scene to load (0 for the first scene)
    public SceneSwitcher sceneSwitcher; // Reference to the SceneSwitcher script

    private void Start()
    {
        StartCoroutine(ResetGameAfterDelay());
    }

    private IEnumerator ResetGameAfterDelay()
    {
        yield return new WaitForSeconds(DelayTime);
        sceneSwitcher.SwitchScene(SceneToLoad);
    }
}
