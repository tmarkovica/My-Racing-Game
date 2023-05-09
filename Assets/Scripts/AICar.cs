using UnityEngine;

public class AICar : MonoBehaviour
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
	private Rigidbody car_RigidBody = null;
	private structAI ai;

	private void Start()
	{
		car_RigidBody = this.GetComponent<Rigidbody>();

		ai.checkpoints = GameObject.FindWithTag("Checkpoints").transform;
		ai.checkpointWallIndex = 0;
	}
	
	private void FixedUpdate()
	{
		// turn
		ai.directionSteer = ai.checkpoints.GetChild(ai.checkpointWallIndex).position - this.transform.position;
		ai.rotationSteer = Quaternion.LookRotation(ai.directionSteer);
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, ai.rotationSteer, turnSpeed);

		// move
		car_RigidBody.AddRelativeForce(Vector3.forward * moveSpeed, ForceMode.VelocityChange);
	}
	
	
	private int CalcNextCheckpoint()
	{		
		int current = ai.checkpointWallIndex;
		int next = ai.checkpointWallIndex + 1;
		if (next > ai.checkpoints.childCount - 1)
			next = 0;
			
		//Debug.Log(string.Format("current checkpoint {0}, next {1}", current, next));
		
		return next;
	}
	
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Wall") == true)
		{
			ai.checkpointWallIndex = CalcNextCheckpoint();
		}
	}
}