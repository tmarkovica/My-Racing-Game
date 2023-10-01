using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class Timer : MonoBehaviour
{	
	private Stopwatch stopwatch = new Stopwatch();
	[SerializeField] Text timerText;
	[SerializeField] Transform player;
	[SerializeField] Transform oponents;
	//private int playerCarPrefabNumber;
	
	//int numberOfOponents = 1;
	
	void Start()
	{
		Time.timeScale = 1;
	}
	
	void FixedUpdate() 
	{
		timerText.text = stopwatch.Elapsed.ToString();		
	}
	
	private void OnEnable()
    {
        TrafficLightController.OnGreenLight += StartStopwatch;
    }

    private void OnDisable()
    {
        TrafficLightController.OnGreenLight -= StartStopwatch;
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
