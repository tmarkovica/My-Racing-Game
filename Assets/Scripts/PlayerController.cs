using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private Rigidbody carRigidBodyRef;
	
	private float speed = 0;
	[SerializeField] float maxForwardSpeed = 700f;
	[SerializeField] float accelerationTime = 5f; // vrijeme u sekundama potrebno za postići maksimalnu brzinu
	private float accelerationRate;
	
	[SerializeField] float breakingStrength = 2.5f;
	[SerializeField] [Range(0, 1)] float minimalBreakingStrengthPercentage = 0.2f;
	
	[SerializeField] Transform leftFrontWheel, rightFrontWheel;
	[SerializeField] float turnStrength = 4f;
	private float maxWheelTurn = 50;
	private float turnInput = 0;
	
	[SerializeField] LayerMask terrainTagForGround;
	public Terrain terrain;
	private bool isGrounded;
	
	[SerializeField] float checkRadius = 2f; //, originalDrag = 9f;
	[SerializeField] float gravityForce = -10f;
	
	private Rigidbody GetRigidBodyComponentFromLastChild() 
	{
		return this.transform.GetChild(this.transform.childCount - 1).GetComponent<Rigidbody>();
	}
	
	void Start()
	{
		//carRigidBodyRef = GetRigidBodyComponentFromLastChild();
		carRigidBodyRef = this.GetComponent<Rigidbody>();
		
		//originalDrag = carRigidBodyRef.drag;
		
		
		accelerationRate = maxForwardSpeed / accelerationTime;
		
		
		leftFrontWheel = this.transform.Find("Front_Left_Wheel");
		rightFrontWheel = this.transform.Find("Front_Right_Wheel");
		Debug.Log(GroundTerrainData.Instance.execute());
		
		terrain = GroundTerrainData.Instance.Terrain;
		terrainTagForGround = GroundTerrainData.Instance.Tag;
	}
		
	
	void Update()
	{	
		OtherControls();
		DrivingControls();		
	}
	
	private void DrivingControls() 
	{
		accelerationRate = maxForwardSpeed / accelerationTime;
		float dotProduct = Vector3.Dot(carRigidBodyRef.velocity, carRigidBodyRef.gameObject.transform.forward);
		
		/* if (!isGrounded)
		{
			//carRigidBodyRef.drag = 0.3f;
			speed = Mathf.Abs(speed) - accelerationRate * Time.deltaTime * (speed > 1 ? 1 : -1);
			return;
		} */

		Debug.Log(speed.ToString());
		
		if (Input.GetAxis("Vertical") > 0) // forward input
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
		else if (Input.GetAxis("Vertical") < 0) // backward input
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
		else // no input
		{			
			if(dotProduct > 0.6f) // decelerating when vehicle is still moving forwards with no throttle
			{
				speed -= accelerationRate * Time.deltaTime;
			} 
			else if (dotProduct < -0.6f) // decelerating when vehicle is still moving backwards with no throttle
			{
				speed += accelerationRate * Time.deltaTime;
			} 
			else 
			{
				//Debug.Log("No input, no speed");
			}	
		}
		//carRigidBodyRef.AddForce(carRigidBodyRef.transform.forward * speed, ForceMode.Acceleration); // selim u fixed update
		speed = Mathf.Clamp(speed, -maxForwardSpeed, maxForwardSpeed);
		
		if (dotProduct > 0.6f || dotProduct < -0.6f)
		{ 

			float speedAsPercentageOfMaxSpeed = speed / maxForwardSpeed;
			
			turnInput = Input.GetAxis("Horizontal") * turnStrength * speedAsPercentageOfMaxSpeed *
			(speedAsPercentageOfMaxSpeed >= (1 - minimalBreakingStrengthPercentage) ? minimalBreakingStrengthPercentage : 1 - speedAsPercentageOfMaxSpeed);
			// ako idem 90% ili više posto maksimalne brzine auta, tada mi ostane samo 10% snage skretanja vozila
			// za, od 0% - 90% maksimalne brzine vozila, odgovara samo komplement postotka za snagu skretanja vozila
			// za ovih 90 - 100 % se ne može iskoristiti komplement jer bi onda totalno oduzeli mogućnost skretanja vozilu
			// možda bolje ovak napisati.. pa onda i teksta prema tomu okrenuti
			//turnInput = Input.GetAxis("Horizontal") * turnStrength *  (speedAsPercentageOfMaxSpeed <= 0.9 ? 1 - speedAsPercentageOfMaxSpeed : 0.1f);
			
		}
		else 
		{
			turnInput = 0;
		}		
		
		/* rigidBodyComponentRef.gameObject.transform.rotation =
		Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput, 0f)); */
		carRigidBodyRef.gameObject.transform.Rotate(new Vector3(0f, turnInput, 0f)); // ovo valja
		//turnInput = Mathf.Clamp(turnInput, -turnStrength, turnStrength);
		
 		// NOVI NAČIN ROTACIJE
		/* if (dotProduct > 0.6f || dotProduct < -0.6f) 
		{
			// Calculate the rotation change based on the steering input
			Quaternion deltaRotation = Quaternion.Euler(0f, turnInput, 0f);
			// Apply the rotation change using Rigidbody's MoveRotation
			carRigidBodyRef.MoveRotation(carRigidBodyRef.rotation * deltaRotation);			
		} */
		
		//Debug.Log(speed.ToString());
	}
	
	private void OtherControls() 
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			carRigidBodyRef.gameObject.transform.localPosition = Vector3.zero;
			carRigidBodyRef.gameObject.transform.localRotation = Quaternion.identity;
			speed = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			speed = 0;
		}
		
		if (Input.GetKeyDown(KeyCode.LeftAlt)) 
		{
			speed = maxForwardSpeed;
		}
	}
	
	private void TurningWheels() 
	{
		leftFrontWheel.localRotation = 
		Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
		rightFrontWheel.localRotation = 
		Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, Input.GetAxis("Horizontal") * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
	}
	
	Vector3 oldPosition;
	
	
	
	void FixedUpdate() 
	{
		TurningWheels();
		
		// ovo za grounded detection mozda i izbaciti
		RaycastHit hit;		
		isGrounded = Physics.Raycast(carRigidBodyRef.transform.position, Vector3.down, out hit, checkRadius, terrainTagForGround);	
		/* 	
		if (isGrounded )	
		{
			debug.text = "grounded";
			//carRigidBodyRef.drag = originalDrag;
			
			//if (Mathf.Abs(speed) > 0)			
				//carRigidBodyRef.AddForce(carRigidBodyRef.transform.forward * speed, ForceMode.Acceleration);
		}
		else 
		{
			debug.text = "not grounded";
			//carRigidBodyRef.drag = 0.1f;
			carRigidBodyRef.AddForce(Vector3.up * gravityForce * 10000f, ForceMode.Acceleration);
			//speed = Mathf.Abs(speed) - accelerationRate * Time.deltaTime * (speed > 1 ? 1 : -1);
		} */
		
		
		carRigidBodyRef.AddForce(carRigidBodyRef.transform.forward * speed, ForceMode.Acceleration);
		
		AdjustSpeedForTerrain();
	}
	
	private void AdjustSpeedForTerrain() 
	{
		maxForwardSpeed = GroundTerrainData.Instance.GetMaxSpeedForTerrainAtPlayersPosition(carRigidBodyRef.transform.position);	
	}
}

