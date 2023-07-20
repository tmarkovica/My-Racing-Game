using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEditor;
using Cinemachine;

public class Timer : MonoBehaviour
{	
	[SerializeField] Text timerText;
	[SerializeField] GameObject car;
	[SerializeField] List<GameObject> aiPlayers;
	
	[SerializeField] GameObject player;
	
	[SerializeField]
	CinemachineVirtualCamera cinemachineVirtualCamera;
	
	private GameObject myCarInstance; // new
	
	private Stopwatch stopwatch = new Stopwatch();
	
	void Awake() 
	{
		UnityEngine.Debug.Log("Awake is called!!!!!");
		
		int carPrefabNumber = PlayerPrefs.GetInt(PlayerPrefsKeys.SelectedCar) + 1;
		GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Racing Cars Pack 1/Prefabs/Car{carPrefabNumber}.prefab");
		myCarInstance = Instantiate(prefab, player.transform, false);
		myCarInstance.transform.localPosition = new Vector3(0, 0.7684599f, 0);
		
		// new
		/* myCarInstance.AddComponent<Rigidbody>();
		myCarInstance.GetComponent<Rigidbody>().mass = 800;
		myCarInstance.GetComponent<Rigidbody>().drag = 4;
		myCarInstance.GetComponent<Rigidbody>().angularDrag = 4;
		myCarInstance.GetComponent<Rigidbody>().useGravity = true;
		myCarInstance.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Extrapolate;
		myCarInstance.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Continuous; */
		
		// new
		cinemachineVirtualCamera.Follow = myCarInstance.transform;
		cinemachineVirtualCamera.LookAt = myCarInstance.transform;
		
	}
	
	void Start()
	{		
		TrafficLightController.OnGreenLight += StartStopwatch;
	}	

	void Update()
	{
		timerText.text = stopwatch.Elapsed.ToString();
	}
	
	bool potroseno = false;
	
	void StartStopwatch() 
	{
		stopwatch.Start();		
		StartRace_EnableScripts();
	}
	
	void StartRace_EnableScripts() 
	{
		this.car.GetComponent<CarController>().enabled = true;
		this.car.GetComponent<RotateWheels>().enabled = true;
		
		foreach (GameObject go in aiPlayers) 
		{
			go.GetComponent<AICar>().enabled = true;
		}
	}
}
