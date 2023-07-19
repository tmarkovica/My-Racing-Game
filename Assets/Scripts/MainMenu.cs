using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{	
	public void PlayButtonClick() 
	{
		SceneManager.LoadScene(GameScenes.CarSelection);
	}
	
	public void ExitButtonClick() 
	{
		Application.Quit();
	}
}
