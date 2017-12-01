using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour 
{

	public void StartButton (string newGameLevel)
	{
		SceneManager.LoadScene(newGameLevel);
	}

	public void ExitButton()
	{
		Application.Quit ();
	}

}
