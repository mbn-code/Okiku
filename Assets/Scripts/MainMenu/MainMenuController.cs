using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public SceneSwitcher sceneSwitcher;

    public void PlayClick()
    {
        sceneSwitcher.SwitchScene(1); // Level 1
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
