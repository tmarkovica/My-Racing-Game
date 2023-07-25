using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarManager : MonoBehaviour
{
	[SerializeField] List<GameObject> prefabCars;
	[SerializeField] PhysicMaterial physicsMaterial;
	
	private GameObject carInstance;	
	private int prefabIndex = 0;	
	private int selectedTrackIndex = 0;
	
	void Awake() 
	{
		InstantiateCar();
	}
	
	private void InstantiateCar() 
	{
		Destroy(carInstance);
		
		carInstance = Instantiate(prefabCars[prefabIndex], this.transform, false);
		carInstance.transform.localPosition = new Vector3(0, 1.5f, 0);
		carInstance.GetComponent<Rigidbody>().drag = 0;
		carInstance.GetComponent<BoxCollider>().material = physicsMaterial;
		carInstance.GetComponent<KeepVehicleGrounded>().enabled = false;
	}	
	
	public void ButtonClick_PreviousCar() 
	{
		prefabIndex--;
		if (prefabIndex < 0)
			prefabIndex = prefabCars.Count - 1;
			
		InstantiateCar();
	}
	
	public void ButtonClicked_NextCar() 
	{
		prefabIndex++;
		if (prefabIndex >= prefabCars.Count)
			prefabIndex = 0;			
		
		InstantiateCar();
	}
	
	public void ButtonClick_GoBack() 
	{
		SceneManager.LoadScene(GameScenes.MainMenu);
	}
	
	public void ButtonClick_StartRace() 
	{
		PlayerPrefs.SetInt(PlayerPrefsKeys.SelectedCar, prefabIndex);
		
		if (selectedTrackIndex == 0)
			SceneManager.LoadScene(GameScenes.Track1);
		
	}
	
	public void Dropdown_TrackSelected(int value) 
	{
		selectedTrackIndex = value;
	}
}
