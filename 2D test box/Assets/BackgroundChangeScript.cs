using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChangeScript : MonoBehaviour
{
    [SerializeField]
    private bool _fadeToBlack = false;
    [SerializeField]
    private bool _destroyGameObjectAfterwards = false;
    [SerializeField]
    private List<GameObject> _disableList;
    [SerializeField]
    private List<GameObject> _enableList;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void Activate()
    {
        if (_fadeToBlack)
            FadeToBlackScript.GetScript().fade = true;
        foreach (GameObject disable in _disableList)
            disable.SetActive(false);
        foreach (GameObject enable in _enableList)
            enable.SetActive(false);

        if (_destroyGameObjectAfterwards)
            Destroy(this.gameObject);
    }
}
