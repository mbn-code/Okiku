using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSceneSwitch : MonoBehaviour
{
    private Animator anm;
    public SceneSwitcher sceneSwitcher;
    public int sceneToLoad = 1; // Default scene to load

    private void Start()
    {
        anm = GetComponent<Animator>();
    }

    private void Update()
    {
        if (anm.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            sceneSwitcher.SwitchFastScene(sceneToLoad);
        }
    }

}
