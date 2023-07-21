using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEditor;
using Cinemachine;

public class Timer : MonoBehaviour
{	
	private Stopwatch stopwatch = new Stopwatch();
	[SerializeField] Text timerText;
	[SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
	[SerializeField] List<GameObject> carPrefabs;
	[SerializeField] GameObject player;
	[SerializeField] GameObject oponents;
	private GameObject playerCarInstance;
	private int playerCarPrefabNumber;
	
	[SerializeField] List<GameObject> aiPlayers;
	
	int numberOfOponents = 1;
	
	void Awake() 
	{		
		/* int carPrefabNumber = PlayerPrefs.GetInt(PlayerPrefsKeys.SelectedCar) + 1;
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Racing Cars Pack 1/Prefabs/Car{carPrefabNumber}.prefab");
		playerCarInstance = Instantiate(prefab, player.transform, false);
		playerCarInstance.transform.localPosition = new Vector3(0, 0.7684599f, 0);
		
		cinemachineVirtualCamera.Follow = playerCarInstance.transform;
		cinemachineVirtualCamera.LookAt = playerCarInstance.transform;
		
		InstantiateOponents(); */
	}
	
	void InstantiateOponents() 
	{
		int spawnedAICarsCount = 0;
		while (spawnedAICarsCount < numberOfOponents) 
		{			
			// zelim bas taj auto radi componenti
			int randomValue = 7;
			GameObject car = Instantiate<GameObject>(carPrefabs[randomValue], oponents.transform, false);
			car.transform.localPosition = new Vector3(0, 0.7684599f, spawnedAICarsCount * -10);
			aiPlayers.Add(car);	
			spawnedAICarsCount++;				
			car.AddComponent<AICar2>();
		}
	}
	
	void Start()
	{		
		TrafficLightController.OnGreenLight += StartStopwatch;
	}
	
	void FixedUpdate() 
	{
		timerText.text = stopwatch.Elapsed.ToString();		
	}
		
	void StartStopwatch() 
	{
		stopwatch.Start();		
		//StartRace_EnableScripts();
	}
	
	void StartRace_EnableScripts() 
	{
		//this.car.GetComponent<CarController>().enabled = true;
		this.playerCarInstance.GetComponent<SpinWheels>().enabled = true;
		
		foreach (GameObject go in aiPlayers) 
		{
			//go.GetComponent<AICar>().enabled = true;
			
			/* GameObject collisionDetection = go.transform.GetChild(go.transform.childCount-1).gameObject;
			collisionDetection.GetComponent<BoxCollider>().enabled = false;			
			collisionDetection.GetComponent<CollisionTryout>().enabled = false; */
		}
		aiPlayers[0].GetComponent<AICar>().enabled = true;
	}
}
