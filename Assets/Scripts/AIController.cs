using UnityEngine;

public class AIController : MonoBehaviour
{
	private struct structAI
	{
		public Transform checkpoints;
		public int checkpointWallIndex;
		public Vector3 directionSteer;
		public Quaternion rotationSteer;
	}

	private Rigidbody carRigidBody = null;
	private structAI ai;	
	
	private float speed = 0;
	private float accelerationTime = 5;
	private float maxForwardSpeed = 700;
	private float accelerationRate;
	
	[SerializeField] float turnStrength = 0.08f;
	
	void Awake()
	{
		Debug.Log("AIController awake.");
		carRigidBody = this.GetComponent<Rigidbody>();

		ai.checkpoints = GameObject.FindWithTag("Checkpoints").transform;
		ai.checkpointWallIndex = 0;		
		
		accelerationRate = maxForwardSpeed / accelerationTime;
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
		this.transform.rotation = Quaternion.Lerp(this.transform.rotation, ai.rotationSteer, turnStrength);
		
		float angle = Vector3.Angle(carRigidBody.transform.forward, ai.checkpoints.GetChild(ai.checkpointWallIndex).position); // ne koristi se		
		
		carRigidBody.AddForce(carRigidBody.transform.forward * speed, ForceMode.Acceleration);
		
		AdjustSpeedForTerrain();
	}	
	
	private void AdjustSpeedForTerrain() 
	{
		maxForwardSpeed = GroundTerrainData.Instance.GetMaxSpeedForTerrainAtPlayersPosition(carRigidBody.transform.position);	
	}
	
	private int CalcNextCheckpoint()
	{		
		int current = ai.checkpointWallIndex;
		int next = ai.checkpointWallIndex + 1;
		if (next > ai.checkpoints.childCount - 1)
			next = 0;
					
		return next;
	}
	
	private void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Wall") == true)
		{
			if (CheckpointsColoring.Instance == null)			
			{
				ai.checkpointWallIndex = CalcNextCheckpoint();
			}
			else
			{
				CheckpointsColoring.Instance.ToggleCheckpointHighlight(ai.checkpoints.GetChild(ai.checkpointWallIndex).transform, false);			
				ai.checkpointWallIndex = CalcNextCheckpoint();
				CheckpointsColoring.Instance.ToggleCheckpointHighlight(ai.checkpoints.GetChild(ai.checkpointWallIndex).transform, true);				
			}
			
		}
	}
}