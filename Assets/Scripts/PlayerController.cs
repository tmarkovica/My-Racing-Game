using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Rigidbody carRigidBody;
	private float speed = 0;
	[SerializeField] float maxSpeed = 700f;
	[SerializeField] float accelerationTime = 5f;
	private float accelerationRate;
	
	[SerializeField] float breakingStrength = 2.5f;
	[SerializeField] [Range(0, 1)] float minimalBreakingStrengthPercentage = 0.2f;
	
	[SerializeField] Transform leftFrontWheel, rightFrontWheel;
	[SerializeField] float turnStrength = 4f;
	private const float maxWheelTurn = 50;
	private float turnInput = 0;
	
	[SerializeField] LayerMask terrainTagForGround;
	public Terrain terrain;
	
	private Rigidbody GetRigidBodyComponentFromLastChild() 
	{
		return this.transform.GetChild(this.transform.childCount - 1).GetComponent<Rigidbody>();
	}
	
	void Start() 
	{
		carRigidBody = this.GetComponent<Rigidbody>();
		
		accelerationRate = maxSpeed / accelerationTime;
		
		
		leftFrontWheel = this.transform.Find("Front_Left_Wheel");
		rightFrontWheel = this.transform.Find("Front_Right_Wheel");
		
		terrain = GroundTerrainData.Instance.Terrain;
		terrainTagForGround = GroundTerrainData.Instance.Tag;
	}
	
	void Update()
	{	
		OtherControls();
		DrivingControls();		
	}

	private const float dotProductTreshold = 0.6f;
	private bool IsMovingForwards(float dotProduct) { return dotProduct > dotProductTreshold; }
	private bool IsMovingBackwards(float dotProduct) { return dotProduct < -dotProductTreshold; }
	
	private void DrivingControls() 
	{
		accelerationRate = maxSpeed / accelerationTime;
		float dotProduct = Vector3.Dot(carRigidBody.velocity, carRigidBody.transform.forward);
		
		if (Input.GetAxis("Vertical") > 0)
		{			
			HandlePositiveVerticalInput();
		}
		else if (Input.GetAxis("Vertical") < 0)
		{			
			HandleNegativeVerticalInput();
		}
		else
		{			
			HandleNoVerticalInput(dotProduct);
		}
		speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
		
		if (IsMovingForwards(dotProduct) || IsMovingBackwards(dotProduct))
		{ 
			AllowSteering();			
		}
		else 
		{
			turnInput = 0;
		}		
		carRigidBody.transform.Rotate(new Vector3(0f, turnInput, 0f));
		
		//turnInput = Mathf.Clamp(turnInput, -turnStrength, turnStrength);
		
 		// NOVI NAČIN ROTACIJE
		/* if (dotProduct > 0.6f || dotProduct < -0.6f) 
		{
			// Calculate the rotation change based on the steering input
			Quaternion deltaRotation = Quaternion.Euler(0f, turnInput, 0f);
			// Apply the rotation change using Rigidbody's MoveRotation
			carRigidBodyRef.MoveRotation(carRigidBodyRef.rotation * deltaRotation);			
		} */
		
		//Debug.Log(dotProduct.ToString());
		//Debug.Log(speed.ToString());
		//Debug.Log(Input.GetAxis("Vertical").ToString());
	}
	
	private void AllowSteering()
	{
		float speedAsPercentageOfMaxSpeed = speed / maxSpeed;
			
		turnInput = Input.GetAxis("Horizontal") * turnStrength * speedAsPercentageOfMaxSpeed *
		(speedAsPercentageOfMaxSpeed >= (1 - minimalBreakingStrengthPercentage) ? minimalBreakingStrengthPercentage : 1 - speedAsPercentageOfMaxSpeed);
		// ako idem 90% ili više posto maksimalne brzine auta, tada mi ostane samo 10% snage skretanja vozila
		// za, od 0% - 90% maksimalne brzine vozila, odgovara samo komplement postotka za snagu skretanja vozila
		// za ovih 90 - 100 % se ne može iskoristiti komplement jer bi onda totalno oduzeli mogućnost skretanja vozilu
		// možda bolje ovak napisati.. pa onda i teksta prema tomu okrenuti
		//turnInput = Input.GetAxis("Horizontal") * turnStrength *  (speedAsPercentageOfMaxSpeed <= 0.9 ? 1 - speedAsPercentageOfMaxSpeed : 0.1f);
	}
	
	private void HandlePositiveVerticalInput()
	{
		if (speed < 0) // breaking
		{
			speed += accelerationRate * Time.deltaTime * breakingStrength;
		}
		else // moving forward, full throttle
		{
			speed += accelerationRate * Time.deltaTime;
		}
	}
	
	private void HandleNegativeVerticalInput()
	{
		if (speed > 0) // breaking
		{
			speed -= accelerationRate * Time.deltaTime * breakingStrength;
		}
		else // going reverse, full throttle
		{
			speed -= accelerationRate * Time.deltaTime;
		}
	}
	
	private void HandleNoVerticalInput(float dotProduct)
	{
		if(IsMovingForwards(dotProduct)) // decelerating when vehicle is still moving forwards with no throttle
		{
			speed -= accelerationRate * Time.deltaTime;
		} 
		else if (IsMovingBackwards(dotProduct)) // decelerating when vehicle is still moving backwards with no throttle
		{
			speed += accelerationRate * Time.deltaTime;
		} 
		else 
		{
			speed = 0;
		}
	}
	
	private void OtherControls() 
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			carRigidBody.transform.localPosition = Vector3.zero;
			carRigidBody.transform.localRotation = Quaternion.identity;
			speed = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			speed = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.LeftAlt)) 
		{
			speed = maxSpeed;
		}
	}
	
	private void TurningWheels() 
	{
		leftFrontWheel.localRotation = 
		Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
		rightFrontWheel.localRotation = 
		Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
	}
	
	void FixedUpdate() 
	{
		TurningWheels();
		
		carRigidBody.AddForce(carRigidBody.transform.forward * speed, ForceMode.Acceleration);		
		AdjustSpeedForTerrain();
	}
	
	private void AdjustSpeedForTerrain() 
	{
		maxSpeed = GroundTerrainData.Instance.GetMaxSpeedForTerrainAtPlayersPosition(carRigidBody.transform.position);	
	}
}

