using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageManager : MonoBehaviour
{
    
    public List<Sprite> spriteList = new List<Sprite>();
    private Image backgroundImage;

    void Awake()
    {
        backgroundImage = GetComponent<Image>();
        Sprite randomImage = spriteList[Random.Range(0, spriteList.Count)];
        backgroundImage.sprite = randomImage;
    }
}
