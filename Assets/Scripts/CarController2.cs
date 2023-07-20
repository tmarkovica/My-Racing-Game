using UnityEngine;
using UnityEngine.UI;

public class CarController2 : MonoBehaviour
{
	private Rigidbody rigidBodyComponentRef;
	
	private float speed = 0;
	[SerializeField] float maxForwardSpeed = 60f, maxReverseSpeed = 5000f;
	[SerializeField] float accelerationTime = 5f; // vrijeme u sekundama potrebno za postići maksimalnu brzinu
	private float accelerationRate;
	
	[SerializeField] float breakingStrength = 2.5f;
	[SerializeField] [Range(0, 1)] float minimalBreakingStrengthPercentage = 0.2f;
	
	[SerializeField] float turnStrength;
	[SerializeField] float maxWheelTurn = 50;
	private float turnInput = 0;
	
	private bool isGrounded;
	[SerializeField] float gravityForce = -10f, checkRadius = 0.5f, drag;
	
	[SerializeField] LayerMask terrainTagForGround;
	
	//public Transform groundCheck; //?
	private Transform leftFrontWheel, rightFrontWheel;
	
	public Terrain terrain;
	public Text debug;
	
	
	private Rigidbody GetRigidBodyComponentFromLastChild() 
	{
		return this.transform.GetChild(this.transform.childCount - 1).GetComponent<Rigidbody>();
	}
	
	void Start()
	{
		Application.targetFrameRate = 120;
		
		//RB.transform.parent = null;
		
		//this.transform.Ge<Rigidbody>()
		rigidBodyComponentRef = GetRigidBodyComponentFromLastChild();
		
		
		accelerationRate = maxForwardSpeed / accelerationTime;
		
		
		leftFrontWheel = this.rigidBodyComponentRef.transform.Find("Front_Left_Wheel");
		rightFrontWheel = this.rigidBodyComponentRef.transform.Find("Front_Right_Wheel");
	}
		
	
	void Update()
	{	
		OtherControls();
		DrivingControls();		
	}
	
	private void DrivingControls() 
	{
		// skalarni produkt dva vektora, 
		float dotProduct = Vector3.Dot(rigidBodyComponentRef.velocity, rigidBodyComponentRef.gameObject.transform.forward);
		
		// izbaciti iz update funkcije, ovdje je da bude praktičnije mijenjati model vožnje, da se rate update-a nakon promjene u editoru
		accelerationRate = maxForwardSpeed / accelerationTime;
		//steerignRate = turnStrength / steeringTime;
		
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
		rigidBodyComponentRef.AddForce(rigidBodyComponentRef.transform.forward * speed, ForceMode.Acceleration);		
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
		/* rigidBodyComponentRef.gameObject.transform.Rotate(new Vector3(0f, turnInput, 0f)); */ // ovo valja
		//turnInput = Mathf.Clamp(turnInput, -turnStrength, turnStrength);
		
 		// NOVI NAČIN ROTACIJE
		if (dotProduct > 0.6f || dotProduct < -0.6f) 
		{
			// Calculate the rotation change based on the steering input
			Quaternion deltaRotation = Quaternion.Euler(0f, turnInput, 0f);
			// Apply the rotation change using Rigidbody's MoveRotation
			rigidBodyComponentRef.MoveRotation(rigidBodyComponentRef.rotation * deltaRotation);			
		}	
	}
	
	private void OtherControls() 
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			rigidBodyComponentRef.gameObject.transform.localPosition = Vector3.zero;
			rigidBodyComponentRef.gameObject.transform.localRotation = Quaternion.identity;
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
	
	[SerializeField] Transform tempTransform;
	
	void FixedUpdate() 
	{
		TurningWheels();
		
		float dotProduct = Vector3.Dot(rigidBodyComponentRef.velocity, rigidBodyComponentRef.gameObject.transform.forward);
		
		
		if (dotProduct > 1 || dotProduct < -1) 
		{
			tempTransform.Rotate(Vector3.right, dotProduct / 1.5f);
		}
		
		
		//rigidBodyComponentRef.AddForce(Vector3.up * gravityForce * 10000f);
		
		//Debug.Log(rigidBodyComponentRef.velocity.magnitude.ToString());
		
		
		/* Vector3 direction = rigidBodyComponentRef.transform.position - oldPosition;
		float forwardTest = Vector3.Dot(-direction.normalized, transform.position.normalized);
 */
 
		// WORKS!
		/* if(rigidBodyComponentRef.velocity.magnitude  > 0) {
			Debug.Log("forward");
		} else if (rigidBodyComponentRef.velocity.magnitude < 0) {
			Debug.Log("backward");
		} else {
			Debug.Log("stopped");
		} */

		//oldPosition = transform.position;
		
		
		/* RaycastHit hit;
		
		isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, checkRadius, terrainTagForGround);
		
		if (isGrounded )
			debug.text = "grounded";
		else
			debug.text = "not grounded";
		
		
		if (isGrounded )
		{
			rigidBodyComponentRef.drag = drag;
			
			if (Mathf.Abs(speedInput) > 0) 
			{
				rigidBodyComponentRef.AddForce(transform.forward * speedInput);
			}
		}
		else 
		{
			rigidBodyComponentRef.drag = 0.1f;
			rigidBodyComponentRef.AddForce(Vector3.up * gravityForce * 100f);
		}		 */
	}
	
	void OnCollisionEnter(Collision collision) {
		
			Debug.Log("OnCollisionEnter: " + collision.contactCount);
			
			
		
		if (collision.gameObject.CompareTag("Player")) {
			Debug.Log("Player hit something");
			
			foreach (ContactPoint contact in collision.contacts) {
				Vector3 hitNormal = contact.normal;
				if (hitNormal == Vector3.up) {
					Debug.Log("Collided with top side");
				}
				else if (hitNormal == Vector3.down) {
					Debug.Log("Collided with bottom side");
				}
				else if (hitNormal == Vector3.right) {
					Debug.Log("Collided with right side");
				}
				else if (hitNormal == Vector3.left) {
					Debug.Log("Collided with left side");
				}
				else if (hitNormal == Vector3.forward) {
					Debug.Log("Collided with front side");
				}
				else if (hitNormal == Vector3.back) {
					Debug.Log("Collided with back side");
				}
			}
		}
	}
	
	void OnTriggerEnter(Collider collider)
	{
		/* Debug.Log(collider.bounds);
		
		// Get the direction of the collision relative to this GameObject
		Vector3 direction = collider.transform.position - transform.position;
		
		//collider.contacts

		// Calculate the dot product of the direction and the transform's forward vector
		float dotProduct = Vector3.Dot(direction, transform.forward);

		if (dotProduct > 0)
		{
			// The player collided with the front side of the box collider
			Debug.Log("Front side collision");
		}
		else if (dotProduct < 0)
		{
			// The player collided with the back side of the box collider
			Debug.Log("Back side collision");
		}
		else
		{
			// The player collided with the exact center of the box collider
			Debug.Log("Center collision");
		} */
		
		
		/* if (collider.CompareTag("Player") == true)
		{
			Debug.Log("Collided");
		} */
	}
}

