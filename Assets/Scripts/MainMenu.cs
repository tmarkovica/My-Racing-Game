using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public Camera cam1;
	public Camera cam2;
	
	public void PlayButtonClick() 
	{
		SceneManager.LoadScene("Track1");
	}
	
	public void CoopButtonClick() 
	{
		Debug.Log(cam1.name + " " + cam1.enabled);
		Debug.Log(cam2.name + " " + cam2.enabled);
		Debug.Log("***************************************");
		DeactivateAllCameras();
		cam1.enabled = true;
		
		Debug.Log(cam1.name + " " + cam1.enabled);
		Debug.Log(cam2.name + " " + cam2.enabled);
	}
	
	public void ExitButtonClick() 
	{
		Debug.Log("Application exitttttttttt");
		Application.Quit();
	}
	
	private void DeactivateAllCameras() 
	{
		cam1.enabled = false;
		cam2.enabled = false;
	}
	
	public void SwitchCameraToNewMenu() 
	{
		Debug.Log(cam1.name + " " + cam1.enabled);
		Debug.Log(cam2.name + " " + cam2.enabled);
		Debug.Log("***************************************");
		DeactivateAllCameras();
		cam2.enabled = true;
		
		Debug.Log(cam1.name + " " + cam1.enabled);
		Debug.Log(cam2.name + " " + cam2.enabled);
	}
}
