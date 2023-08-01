using UnityEngine;

public class PlayerControllerMobile : MonoBehaviour
{
	private Rigidbody carRigidBody;
	public CarSpecifications specs;
	private float maxSpeed;
	private float speed = 0;
	private float accelerationRate;
	
	[SerializeField] Transform leftFrontWheel, rightFrontWheel;
	
	private bool mobile = false;
	
	private Rigidbody GetRigidBodyComponentFromLastChild() 
	{
		return this.transform.GetChild(this.transform.childCount - 1).GetComponent<Rigidbody>();
	}
	
	void Awake()
	{
		carRigidBody = this.GetComponent<Rigidbody>();
		
		leftFrontWheel = this.transform.Find("Front_Left_Wheel");
		rightFrontWheel = this.transform.Find("Front_Right_Wheel");
	}
	
	void Start() 
	{		
		maxSpeed = specs.maxSpeed;
		accelerationRate = maxSpeed / specs.accelerationTime;
		
		
		
		#if UNITY_ANDROID || UNITY_WEBGL
			Input.gyro.enabled = true;
		#endif
		
		if (Application.platform == RuntimePlatform.Android)
			mobile = true;
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
		accelerationRate = maxSpeed / specs.accelerationTime;
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
		/* if (mobile) speed = maxSpeed; */
		
		if (IsMovingForwards(dotProduct) || IsMovingBackwards(dotProduct))
		{ 
			HandleSteering();
		}
	}
	
	private void HandleSteering()
	{	
		Vector3 originalForward = transform.forward;
		Vector3 directionSteer = Quaternion.AngleAxis(specs.maxWheelTurn_deg * Input.GetAxis("Horizontal"), Vector3.up) * originalForward;
		Quaternion rotationSteer = Quaternion.LookRotation(directionSteer);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotationSteer, specs.turnStrength * Time.deltaTime);
	}
	
	private void HandlePositiveVerticalInput()
	{
		if (speed < 0) // breaking
		{			
			speed += accelerationRate * Time.deltaTime * specs.breakingStrength;
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
			speed -= accelerationRate * Time.deltaTime * specs.breakingStrength;
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
		Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * specs.maxWheelTurn_deg, leftFrontWheel.localRotation.eulerAngles.z);
		rightFrontWheel.localRotation = 
		Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * specs.maxWheelTurn_deg, rightFrontWheel.localRotation.eulerAngles.z);
	}
	
	void FixedUpdate() 
	{		
		carRigidBody.AddForce(carRigidBody.transform.forward * speed, ForceMode.Acceleration);
		AdjustSpeedForTerrain();
		TurningWheels();
	}
	
	private void AdjustSpeedForTerrain() 
	{
		maxSpeed = GroundTerrainData.Instance.GetPercentageOfMaxSpeedAllowedAt(carRigidBody.transform.position) * specs.maxSpeed;	
	}
}

