using UnityEngine;
using UnityEngine.UI;

public class CarSpecs : MonoBehaviour
{
	private Rigidbody carRigidBodyRef;
	[SerializeField] float maxSpeed = 700f;
	[SerializeField] float accelerationTime = 5f; // vrijeme u sekundama potrebno za postići maksimalnu brzinu
	private float accelerationRate;
	
	[SerializeField] float breakingStrength = 2.5f;
	[SerializeField] [Range(0, 1)] float minimalBreakingStrengthPercentage = 0.2f;

	[SerializeField] float turnStrength;
	public float maxWheelTurn = 50;
	
	[SerializeField] LayerMask terrainTagForGround;
	public Terrain terrain;
	
	[SerializeField] float gravityForce = -10f, checkRadius = 2f, originalDrag = 9f;
	
	void Start() 
	{
		accelerationRate = maxSpeed / accelerationTime;
		
		
	}
}

