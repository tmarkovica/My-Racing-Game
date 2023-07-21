using UnityEngine;
using UnityEngine.UI;

public class CarController2 : MonoBehaviour
{
	private Rigidbody carRigidBodyRef;
	
	private float speed = 0;
	[SerializeField] float maxForwardSpeed = 60f, maxReverseSpeed = 5000f;
	[SerializeField] float accelerationTime = 5f; // vrijeme u sekundama potrebno za postići maksimalnu brzinu
	private float accelerationRate;
	
	[SerializeField] float breakingStrength = 2.5f;
	[SerializeField] [Range(0, 1)] float minimalBreakingStrengthPercentage = 0.2f;
	
	[SerializeField] Transform leftFrontWheel, rightFrontWheel;
	[SerializeField] float turnStrength;
	private float maxWheelTurn = 50;
	private float turnInput = 0;
	
	[SerializeField] LayerMask terrainTagForGround;
	public Terrain terrain;
	private bool isGrounded;
	
	[SerializeField] float gravityForce = -10f, checkRadius = 2f, originalDrag = 4f;
	
	public Text debug;
	
	private Rigidbody GetRigidBodyComponentFromLastChild() 
	{
		return this.transform.GetChild(this.transform.childCount - 1).GetComponent<Rigidbody>();
	}
	
	void Start()
	{
		carRigidBodyRef = GetRigidBodyComponentFromLastChild();
		originalDrag = carRigidBodyRef.drag;
		
		
		accelerationRate = maxForwardSpeed / accelerationTime;
		
		
		leftFrontWheel = this.carRigidBodyRef.transform.Find("Front_Left_Wheel");
		rightFrontWheel = this.carRigidBodyRef.transform.Find("Front_Right_Wheel");
	}
		
	
	void Update()
	{	
		OtherControls();
		DrivingControls();		
	}
	
	private void DrivingControls() 
	{
		// skalarni produkt dva vektora, 
		float dotProduct = Vector3.Dot(carRigidBodyRef.velocity, carRigidBodyRef.gameObject.transform.forward);
		
		// izbaciti iz update funkcije, ovdje je da bude praktičnije mijenjati model vožnje, da se rate update-a nakon promjene u editoru
		accelerationRate = maxForwardSpeed / accelerationTime;
		//steerignRate = turnStrength / steeringTime;
		
		if (!isGrounded)
		{
			//carRigidBodyRef.drag = 0.3f;
			speed = Mathf.Abs(speed) - accelerationRate * Time.deltaTime * (speed > 1 ? 1 : -1);
			return;
		}
		
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
		DetectTexture();		
	}
	
	void DetectTexture() 
	{
		// Get the player's position in world space.
		Vector3 playerPosition = carRigidBodyRef.transform.position;

		// Ensure the player's position is within the bounds of the terrain.
		if (terrain.terrainData.bounds.Contains(playerPosition))
		{
			// Get the normalized position of the player within the terrain.
			Vector3 normalizedPosition = GetNormalizedTerrainPosition(playerPosition);

			// Get the alphamap data at the player's position.
			float[,,] alphamapData = terrain.terrainData.GetAlphamaps(
				(int)(normalizedPosition.x * terrain.terrainData.alphamapWidth),
				(int)(normalizedPosition.z * terrain.terrainData.alphamapHeight),
				1, 1
			);

			// Find the dominant texture at the player's position.
			int textureIndex = GetDominantTextureIndex(alphamapData);

			if (textureIndex == 0) // grass
			{
				maxForwardSpeed = 400;
			}
			else // road
			{
				maxForwardSpeed = 700;
			}			

			//Debug.Log("Player is on texture index: " + textureIndex);
		}
	}
	
	private Vector3 GetNormalizedTerrainPosition(Vector3 position)
	{
		// Convert world position to normalized position within the terrain.
		Vector3 normalizedPosition = new Vector3(
			(position.x - terrain.terrainData.bounds.min.x) / terrain.terrainData.size.x,
			0,
			(position.z - terrain.terrainData.bounds.min.z) / terrain.terrainData.size.z
		);

		return normalizedPosition;
	}

	private int GetDominantTextureIndex(float[,,] alphamapData)
	{
		// Find the index of the highest value in the alphamap data.
		int dominantIndex = 0;
		float maxAlpha = 0f;

		for (int i = 0; i < alphamapData.GetLength(2); i++)
		{
			if (alphamapData[0, 0, i] > maxAlpha)
			{
				maxAlpha = alphamapData[0, 0, i];
				dominantIndex = i;
			}
		}

		return dominantIndex;
	}
}

