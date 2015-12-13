using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	// Update is called once every frame
	public void Update()
	{

	}

	public void StartGame()
	{
		EditorSceneManager.LoadScene ("Test");
	}

    public void OpenCredits()
    {
        EditorSceneManager.LoadScene("CreditsScene");
    }

    public void QuitGame()
	{
		Application.Quit ();
	}

    public void GoBackToMainMenu()
    {
        EditorSceneManager.LoadScene("MainScene");
    }
}
