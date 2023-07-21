using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{	
	public void PlayButtonClick() 
	{
		SceneManager.LoadScene(GameScenes.CarSelection);
	}
	
	public void QuitButtonClick() 
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
				Application.Quit();
		#endif
	}
}
