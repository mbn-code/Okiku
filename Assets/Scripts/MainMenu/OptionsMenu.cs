using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public TMP_Text volumeText;
    public Slider volumeSlider;

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(delegate { volumeSliderChanged(); });
    }

    public void volumeSliderChanged()
    {
        volumeText.text = volumeSlider.value.ToString() + "%";
    }
}
