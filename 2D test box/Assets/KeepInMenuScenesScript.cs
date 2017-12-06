using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeepInMenuScenesScript : MonoBehaviour
{

    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        if (arg1.name != "Lab" && arg1.name != "Menu2 (1)")
            Destroy(gameObject);
    }

    private void Update()
    {

    }
}
