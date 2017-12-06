using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeToBlackScript : MonoBehaviour
{
    private static FadeToBlackScript _singletonInstance;
    private bool _fade = false;
    private SpriteRenderer _sprite;
    private string nextScene = "Lab";

    public bool fade
    { set { _fade = value; } }

    private void Start()
    {
        if (_singletonInstance != null && _singletonInstance != this.gameObject)
            Destroy(gameObject);
        else
            _singletonInstance = this;

        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(_fade)
        {
            Color color = _sprite.color;
            color.a += 0.01f;
            _sprite.color = color;
            if(color.a >= 1.0f)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    public void SetNextScene(string sceneName)
    {
        nextScene = sceneName;
    }

    public static FadeToBlackScript GetScript()
    {
        return _singletonInstance;
    }
}
