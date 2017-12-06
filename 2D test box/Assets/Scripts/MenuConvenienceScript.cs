using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuConvenienceScript : MonoBehaviour
{
    public GameObject objectToEnable;
    public Image image;
    public Sprite spriteOne;
    public Sprite spriteTwo;
    public Sprite spriteThree;

    private bool _spriteSwitch = true;

    public void EnableObject()
    {
        if (objectToEnable != null)
            objectToEnable.SetActive(!objectToEnable.activeSelf);//!objectToEnable.activeSelf);
    }

    public void SwitchSprites()
    {
        if (image != null && spriteOne != null && spriteTwo != null)
        {
            if (_spriteSwitch)
                image.sprite = spriteTwo;
            else
                image.sprite = spriteOne;
            _spriteSwitch = !_spriteSwitch;
        }
    }

    public void RotateSprites()
    {
        if (image != null && spriteOne != null && spriteTwo != null && spriteThree != null)
        {
            if (image.sprite == spriteOne)
                image.sprite = spriteTwo;
            else if (image.sprite == spriteTwo)
                image.sprite = spriteThree;
            else if (image.sprite == spriteThree)
                image.sprite = spriteOne;
        }
    }
}
