using UnityEngine;

public class PlatformDetection : MonoBehaviour
{
	public bool IsMobile { get; private set; }
	
	GameObject forwardPedal, reversePedal;
	
	void Start()
	{
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
		{
			IsMobile = true;
			Debug.Log("Running on a mobile platform.");
		}
		else
		{
			IsMobile = false;
			Debug.Log("Running on a desktop platform.");
		}
	}
	
	public void SimulatePositiveVerticalInput()
	{
		
	}
	
	void SimulateNegativeVerticalInput()
	{
		
	}
}
