using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
	[SerializeField] AudioSource SFXPlayer;
	[SerializeField] GameObject menuPanel;
	
	void Start() 
	{
	}
	
	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Escape)) 
		{
			toggleGamePause();
		}
	}
	
	private void toggleGamePause() 
	{
		bool paused = (Time.timeScale != 0);
		Time.timeScale = paused == true ? 0 : 1;
		SFXPlayer.mute = paused;
		menuPanel.SetActive(paused);
	}
	
	public void RestartGame() 
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	
	public void CarSelection()
	{
		SceneManager.LoadScene(GameScenes.CarSelection);
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