using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameHandler : MonoBehaviour
{
	[SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] List<GameObject> carPrefabs;	
	[SerializeField] Transform player;
	[SerializeField] Transform oponents;
	
	private GameObject playerCarInstance;
	private List<GameObject> aiCarInstances = new List<GameObject>();
	
	int numberOfOponents = 1;
	private int playerCarPrefabNumber; // temp
	
	void Awake()
	{
		int carPrefabNumber = PlayerPrefs.GetInt(PlayerPrefsKeys.SelectedCar);
		/* 
		playerCarInstance = Instantiate(carPrefabs[carPrefabNumber], player.transform, false);
		playerCarInstance.transform.localPosition = new Vector3(0, 0.7684599f, 0); 
		*/		
		playerCarInstance = InstantiateCar(carPrefabs[carPrefabNumber], player, new Vector3(0, 0.7684599f, 0));
		playerCarInstance.AddComponent<PlayerController>();
		
		cinemachineVirtualCamera.Follow = playerCarInstance.transform;
		cinemachineVirtualCamera.LookAt = playerCarInstance.transform;
		
		int spawnedAICarsCount = 0;
		while (spawnedAICarsCount < numberOfOponents) 
		{
			/* int randomValue = Random.Range(0, 8);
			if (randomValue != playerCarPrefabNumber)			
			{
				GameObject car = Instantiate<GameObject>(carPrefabs[randomValue], oponents.transform, false);
				car.transform.localPosition = new Vector3(0, 0.7684599f, spawnedAICarsCount * -10);
				aiPlayers.Add(car);	
				spawnedAICarsCount++;				
				car.AddComponent<AICar2>();
			} */
			
			// zelim bas taj auto radi componenti
			int randomValue = 7;
			GameObject car = InstantiateCar(carPrefabs[randomValue], oponents, new Vector3(0, 0.7684599f, spawnedAICarsCount * -10));
			aiCarInstances.Add(car);	
			spawnedAICarsCount++;				
			car.AddComponent<AIController>();
		}
		
		//InstantiateOponents();
	}
	
	private GameObject InstantiateCar(GameObject prefab, Transform parent, Vector3 positionRelativeToParent)
	{
		GameObject car = Instantiate(prefab, parent, false);
		car.transform.localPosition = positionRelativeToParent;
		return car;
	}
}
