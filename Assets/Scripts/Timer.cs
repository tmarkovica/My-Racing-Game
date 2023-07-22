using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{	
	private Stopwatch stopwatch = new Stopwatch();
	[SerializeField] Text timerText;
	[SerializeField] Transform player;
	[SerializeField] Transform oponents;
	private int playerCarPrefabNumber;
	
	[SerializeField] List<Transform> aiPlayers;
	
	int numberOfOponents = 1;
	
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
		StartRace_EnableScripts();
	}
	
	void StartRace_EnableScripts() 
	{
		this.player.GetChild(0).GetComponent<PlayerController>().enabled = true;
		
		for (int i = 0; i < oponents.childCount; i++)
		{
			oponents.GetChild(i).GetComponent<AIController>().enabled = true;
		}
	}
}
