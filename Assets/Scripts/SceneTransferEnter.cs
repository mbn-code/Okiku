using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransferEnter : MonoBehaviour
{
    public int SceneId;
    public SceneSwitcher sceneSwitcher;
    public bool isFastSceneSwitch = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other != null)
        {
            if(other.gameObject.tag == "Player")
            {
                if (isFastSceneSwitch)
                    sceneSwitcher.SwitchFastScene(SceneId);
                else
                    sceneSwitcher.SwitchScene(SceneId);
            }
        }
    }
}
