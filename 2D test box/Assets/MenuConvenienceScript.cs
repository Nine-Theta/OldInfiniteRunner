using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuConvenienceScript : MonoBehaviour
{
    public GameObject objectToEnable;

    public void EnableObject()
    {
        objectToEnable.SetActive(!objectToEnable.activeSelf);//!objectToEnable.activeSelf);
    }
}
