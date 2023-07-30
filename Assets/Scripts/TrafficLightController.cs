using UnityEngine;

public class TrafficLightController : MonoBehaviour
{
	[SerializeField] Light red, yellow, green;
	private AudioSource audioSrc;
	
	private float timeInterval = 1.5f;
	
	public delegate void GreenLight();
	public static event GreenLight OnGreenLight;
	
	void Start()
	{
		Debug.Log("TrafficLightController started");
		audioSrc = this.GetComponent<AudioSource>();
		
		red.enabled = true;		
		audioSrc.Play();
		InvokeRepeating("TurnOffRedLight", timeInterval, 0);		
	}
	
	private void TurnOffRedLight()
	{		
		red.enabled = false;
		yellow.enabled = true;
		audioSrc.Play();
		InvokeRepeating("TurnOffYellowLight", timeInterval, 0);
	}
	
	private void TurnOffYellowLight()
	{
		yellow.enabled = false;
		green.enabled = true;
		audioSrc.Play();
		OnGreenLight();
		InvokeRepeating("TurnOffGreenLight", timeInterval, 0);
	}
	
	private void TurnOffGreenLight()
	{		
		green.enabled = false;
	}
}
