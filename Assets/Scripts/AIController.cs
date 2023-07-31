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
	private BoxCollider boxCollider;
	private structAI ai;
	
	public CarSpecifications specs;
	
	private float maxSpeed = 700;
	private float speed = 0;
	private float accelerationRate;
	
	void Awake()
	{
		carRigidBody = this.GetComponent<Rigidbody>();
		boxCollider = this.GetComponent<BoxCollider>();

		ai.checkpoints = GameObject.FindWithTag("Checkpoints").transform;
		ai.checkpointWallIndex = 0;		
	}
	
	void Start()
	{
		maxSpeed = specs.maxSpeed;
		accelerationRate = maxSpeed / specs.accelerationTime;
	}
	
	void Update() 
	{
		speed += accelerationRate * Time.deltaTime;
		speed = Mathf.Clamp(speed, 0, maxSpeed);
		
		HandleSteering();
	}
	
	private void HandleSteering()
	{
		ai.directionSteer = ai.checkpoints.GetChild(ai.checkpointWallIndex).position - this.transform.position;
		ai.rotationSteer = Quaternion.LookRotation(ai.directionSteer);
		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, ai.rotationSteer, specs.turnStrength * Time.deltaTime);
	}
	
	private void FixedUpdate()
	{		
		carRigidBody.AddForce(carRigidBody.transform.forward * speed, ForceMode.Acceleration);
		AdjustSpeedForTerrain();
	}	
	
	private void AdjustSpeedForTerrain() 
	{
		maxSpeed = GroundTerrainData.Instance.GetPercentageOfMaxSpeedAllowedAt(carRigidBody.transform.position) * specs.maxSpeed;	
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