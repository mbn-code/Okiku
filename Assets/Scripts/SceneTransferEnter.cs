using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransferEnter : MonoBehaviour
{
    public int SceneId;
    public SceneSwitcher sceneSwitcher;
    public bool isFastSceneSwitch = false;
    public bool isDelayedSceneSwitch = false;
    public float delayTime = 1f;

    private IEnumerator DelayedSwitch(int sceneId)
    {
        yield return new WaitForSeconds(delayTime);
        sceneSwitcher.SwitchScene(sceneId);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.gameObject.tag == "Player")
            {
                if (isFastSceneSwitch)
                {
                    sceneSwitcher.SwitchFastScene(SceneId);
                }
                else
                {
                    if (isDelayedSceneSwitch)
                        StartCoroutine(DelayedSwitch(SceneId));
                    else
                        sceneSwitcher.SwitchScene(SceneId);
                }
            }
        }
    }
}
