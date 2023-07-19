using UnityEngine;

public class RotateWheels : MonoBehaviour
{
	[SerializeField]
	private Transform leftFrontWheel, rightFrontWheel, leftRearWheel, rightRearWheel;
	
	[SerializeField] WheelCollider frontLeft;
	[SerializeField] WheelCollider frontRight;
	[SerializeField] WheelCollider rearLeft;
	[SerializeField] WheelCollider rearRight;
	
	private Vector3 lastPosition;
	
	public float movementThreshold = 0.01f;
	public float rotationSpeed = 10f;

	private void Start()
	{
		lastPosition = transform.position;
	}

	private void Update()
	{
		float distance = Vector3.Distance(transform.position, lastPosition);
		if (distance > movementThreshold)
		{
			/* leftFrontWheel.RotateAround(transform.position, Vector3.right, rotationSpeed * Time.deltaTime);
			rightFrontWheel.RotateAround(transform.position, Vector3.right, rotationSpeed * Time.deltaTime);
			leftRearWheel.RotateAround(transform.position, Vector3.right, rotationSpeed * Time.deltaTime);
			rightRearWheel.RotateAround(transform.position, Vector3.right, rotationSpeed * Time.deltaTime); */
			/* leftFrontWheel.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
			rightFrontWheel.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
			leftRearWheel.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
			rightRearWheel.Rotate(Vector3.right * rotationSpeed * Time.deltaTime); */			
		}
		//lastPosition = transform.position;
		
		
		
		/* frontLeft.motorTorque = 0f;
		frontRight.motorTorque = 0f;
		
		SpinWheel(frontLeft, leftFrontWheel);
		SpinWheel(frontRight, rightFrontWheel);
		SpinWheel(rearLeft, leftRearWheel);
		SpinWheel(rearRight, rightRearWheel); */
		
		float rotationAmount = rotationSpeed * Time.deltaTime;

		// Create a quaternion representing the desired rotation
		Quaternion rotation = Quaternion.AngleAxis(rotationAmount, Vector3.right);

		// Apply the rotation to the game object
		rightFrontWheel.rotation *= rotation;
		rightRearWheel.rotation = rotation;
	}
	
	void SpinWheel(WheelCollider collider, Transform wheelTransform) 
	{
		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);
		//wheelTransform.position = position;
		wheelTransform.rotation = rotation;
	}
}
