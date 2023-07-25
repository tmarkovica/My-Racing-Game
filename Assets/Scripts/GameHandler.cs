using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] List<GameObject> carPrefabs;	
	private int playerCarPrefabNumber = 7;
	[SerializeField] Transform player;
	[SerializeField] Transform oponents;	
	private GameObject playerCarInstance;
	private List<GameObject> aiCarInstances = new List<GameObject>();
	
	/* void Awake()
	{
		InstantiatePlayerVehicle();
		InstantiateOponentVehicles(1);
	} */
	
	void Start() 
	{
		InstantiatePlayerVehicle();
		InstantiateOponentVehicles(1);
	}
	
	private void SetCinemachineCameraToFollow(Transform transform) 
	{
		cinemachineVirtualCamera.Follow = transform.transform;
		cinemachineVirtualCamera.LookAt = transform.transform;
	}
	
	private GameObject InstantiateCar(GameObject prefab, Transform parent, Vector3 positionRelativeToParent)
	{
		GameObject car = Instantiate(prefab, parent, false);
		car.transform.localPosition = positionRelativeToParent;
		return car;
	}
	
	private void InstantiatePlayerVehicle()
	{
		int playersCarPrefabIndex = PlayerPrefs.GetInt(PlayerPrefsKeys.SelectedCar);	
		playerCarInstance = InstantiateCar(carPrefabs[playersCarPrefabIndex], player, new Vector3(0, 0.7684599f, 0));
		playerCarInstance.AddComponent<PlayerController>().enabled = false;
		SetCinemachineCameraToFollow(playerCarInstance.transform);
	}
	
	private void InstantiateOponentVehicles(int numberOfOponents) 
	{
		int spawnedAICarsCount = 0;
		
		while (spawnedAICarsCount < numberOfOponents) 
		{
			int randomValue = Random.Range(0, 8);	
			if (randomValue != playerCarPrefabNumber)			
			{
				GameObject car = Instantiate<GameObject>(carPrefabs[randomValue], oponents.transform, false);
				car.transform.localPosition = new Vector3(0, 0.7684599f, spawnedAICarsCount * -10);
				aiCarInstances.Add(car);	
				spawnedAICarsCount++;				
				car.AddComponent<AIController>().enabled = false;
				//SetCinemachineCameraToFollow(car.transform);
			}
		}
	}
}
