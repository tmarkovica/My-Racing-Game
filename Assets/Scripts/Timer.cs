using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
	[SerializeField] Text timerText;
	[SerializeField] GameObject player;
	[SerializeField] List<GameObject> aiPlayers;
	
	private Stopwatch stopwatch = new Stopwatch();
	
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
		this.player.GetComponent<CarController>().enabled = true;
		this.player.GetComponent<RotateWheels>().enabled = true;
		
		foreach (GameObject go in aiPlayers) 
		{
			go.GetComponent<AICar>().enabled = true;
		}
	}
}
