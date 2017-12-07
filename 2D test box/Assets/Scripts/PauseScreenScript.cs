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
        Time.timeScale = 1.0f;
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
        if (Time.timeScale > 0.0f)
            Time.timeScale = 0.0f;
        else
            Time.timeScale = 1.0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCanvas();
        }
    }
}
