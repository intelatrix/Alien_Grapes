using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	// Update is called once every frame
	public void Update()
	{

	}

	public void StartGame()
	{
		Application.LoadLevel ("GameScene");
	}

    public void OpenCredits()
    {
        Application.LoadLevel("CreditsScene");
    }

    public void QuitGame()
	{
		Application.Quit ();
	}

    public void GoBackToMainMenu()
    {
        Application.LoadLevel("MainScene");
    }
}
