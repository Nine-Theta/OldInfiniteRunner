using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseScreenScript : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;

    public void LoadScene(string pScene)
    {
        SceneManager.LoadScene(pScene);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ToggleCanvas()
    {
        _pauseCanvas.SetActive(!_pauseCanvas.activeSelf);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }
}
