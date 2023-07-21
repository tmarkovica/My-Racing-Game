using UnityEngine;

public class VectorRotationTest : MonoBehaviour
{
	GameObject red;
	Vector3 redOriginalPosition;
	Quaternion redOriginalRotation;
	
	GameObject green;
	
	public float speed = 100;
	
	// Start is called before the first frame update
	void Start()
	{
		red = this.transform.GetChild(0).gameObject;
		redOriginalPosition = red.transform.position;
		redOriginalRotation = red.transform.rotation;
		
		green = this.transform.GetChild(1).gameObject;
		
		Vector3 directionSteer =  green.transform.position - red.transform.position;
		Quaternion rotationSteer = Quaternion.LookRotation(directionSteer);
		
		Debug.Log(directionSteer.ToString());
		Debug.Log(rotationSteer.ToString());
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			
			Vector3 directionSteer =  green.transform.position - red.transform.position;
			Quaternion rotationSteer = Quaternion.LookRotation(directionSteer);
			
			red.transform.rotation = Quaternion.Lerp(red.transform.rotation, rotationSteer, speed);
		}
		
		if (Input.GetKeyDown(KeyCode.Q))
		{
			red.transform.position = redOriginalPosition;
			red.transform.rotation = redOriginalRotation;
		}
		
		if (Input.GetKeyDown(KeyCode.Z)) 
		{
			red.GetComponent<Rigidbody>().AddForce(red.transform.forward * speed, ForceMode.Acceleration);
		}
		
		if (Input.GetKeyDown(KeyCode.Y)) 
		{
			red.GetComponent<Rigidbody>().AddForce(red.transform.up * speed, ForceMode.Acceleration);
		}
		
		if (Input.GetKeyDown(KeyCode.X)) 
		{
			red.GetComponent<Rigidbody>().AddForce(red.transform.right * speed, ForceMode.Acceleration);
		}
	}
}
