using UnityEngine;

public class AICar2 : MonoBehaviour
{
	private struct structAI
	{
		public Transform checkpoints;
		public int checkpointWallIndex;
		public Vector3 directionSteer;
		public Quaternion rotationSteer;
	}

	public float moveSpeed = 1.0f;
	public float turnSpeed = 90f;
	private Rigidbody carRigidBody = null;
	private structAI ai;
	
	
	private float speed = 0;
	private float accelerationTime = 5;
	private float maxForwardSpeed = 700;
	private float accelerationRate;

	private void Start()
	{
		carRigidBody = this.GetComponent<Rigidbody>();

		ai.checkpoints = GameObject.FindWithTag("Checkpoints").transform;
		Debug.Log("Number of checkpoints = " + ai.checkpoints.childCount.ToString());
		ai.checkpointWallIndex = 0;
		
		
		accelerationRate = maxForwardSpeed / accelerationTime;
		
		
		
		GameObject collisionDetection = this.transform.GetChild(this.transform.childCount-1).gameObject;
		collisionDetection.GetComponent<CollisionTryout>().enabled = false; // iskljucim triggera tu, koristim ovoga u AICar2
		//collisionDetection.GetComponent
	}
	
	void Update() 
	{
		speed += accelerationRate * Time.deltaTime;
		speed = Mathf.Clamp(speed, 0, maxForwardSpeed);
	}
	
	private void FixedUpdate()
	{
		// turn
		ai.directionSteer = ai.checkpoints.GetChild(ai.checkpointWallIndex).position - this.transform.position;
		ai.rotationSteer = Quaternion.LookRotation(ai.directionSteer);
		//ai.rotationSteer = Quaternion.LookRotation(ai.checkpoints.GetChild(ai.checkpointWallIndex).position);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, ai.rotationSteer, speed);
		
		
		
		/* float distanceToCheckpoint = Vector3.Distance(this.transform.position, ai.checkpoints.GetChild(ai.checkpointWallIndex).position);

		// Check if the car has passed the current checkpoint.
		if (distanceToCheckpoint < 1.0f) // You can adjust the distance threshold as needed.
		{
			// Increment to the next checkpoint.
			ai.checkpointWallIndex = CalcNextCheckpoint();
		} */
		
		
		
		
		float angle = Vector3.Angle(carRigidBody.transform.forward, ai.checkpoints.GetChild(ai.checkpointWallIndex).position);

		// move
		//car_RigidBody.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.VelocityChange);
		
		
		carRigidBody.AddForce(carRigidBody.transform.forward * speed, ForceMode.Acceleration);
	}
	
	void LateUpdate() 
	{
	}
	
	
	private int CalcNextCheckpoint()
	{		
		int current = ai.checkpointWallIndex;
		int next = ai.checkpointWallIndex + 1;
		if (next > ai.checkpoints.childCount - 1)
			next = 0;
			
		Debug.Log(string.Format("current checkpoint {0}, next {1}", current, next));
		
		return next;
	}
	
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Wall") == true)
		{			
			ai.checkpointWallIndex = CalcNextCheckpoint();
		}
	}
	
	/* void OnTriggerExit(Collider collider)
	{
		if (collider.CompareTag("Wall") == true)
		{			
			ai.checkpointWallIndex = CalcNextCheckpoint();
		}
	} */
}