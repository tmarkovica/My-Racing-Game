using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
	[SerializeField] AudioSource SFXPlayer;
	[SerializeField] GameObject menuPanel;
	
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			ToggleGamePause();
		}
	}
	
	private void ToggleGamePause() 
	{
		bool paused = (Time.timeScale != 0);
		Time.timeScale = paused == true ? 0 : 1;
		SFXPlayer.mute = paused;
		menuPanel.SetActive(paused);
	}
	
	public void RestartGame() 
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
	
	public void CarSelection()
	{
		SceneManager.LoadScene(GameScenes.CarSelection, LoadSceneMode.Single);
	}
	
	public void QuitGame() 
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}
}