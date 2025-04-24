using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionMenu;

    public void PlayClick()
    {
        // SceneManager til sidste saved scene
    }

    public void OptionsClick()
    {
        mainMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void OptionsBackClick()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitClick()
    {
        Application.Quit();
    }
}
