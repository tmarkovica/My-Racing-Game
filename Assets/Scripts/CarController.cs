using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
	private Rigidbody rigidBodyComponentRef;
	
	public float forwardAcceleration = 0f, reverseAcceleration, turnStrength, gravityForce = -10f, checkRadius = 0.5f, drag, maxWheelTurn;
	
	public float maxForwardSpeed = 10000f, maxReverseSpeed = 5000f;
	
	private float speedInput, turnInput;
	
	private bool isGrounded;
	
	[SerializeField] LayerMask terrainTagForGround;
	
	//public Transform groundCheck; //?
	public Transform leftFrontWheel, rightFrontWheel;
	
	public float smoothingFactor = 0.1f; // novo
	public float targetSpeedInput = 0f; // novo
	public Terrain terrain;
	public Text debug;
	
	void Start()
	{
		//RB.transform.parent = null;
		rigidBodyComponentRef = this.GetComponent<Rigidbody>();
	}

	void Update()
	{
		speedInput = 0f;
		
		if (Input.GetAxis("Vertical") > 0) 
		{
			/* forwardAcceleration =  Mathf.Lerp(forwardAcceleration, 15.0f, Time.deltaTime);
			Debug.Log(Time.deltaTime + " ; " + forwardAcceleration); */			
			
			speedInput = Input.GetAxis("Vertical") * forwardAcceleration * maxForwardSpeed;
			
		}
		else if (Input.GetAxis("Vertical") < 0) 
		{
			speedInput = Input.GetAxis("Vertical") * reverseAcceleration * maxReverseSpeed;
		}
		
		turnInput = Input.GetAxis("Horizontal");
		
		if (isGrounded) 
		{
			transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * Input.GetAxis("Vertical"), 0f));			
		}
		
		leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, leftFrontWheel.localRotation.eulerAngles.z);
		rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);
		
		transform.position = rigidBodyComponentRef.transform.position;
		
		
		
		if (Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("Reset position!");
		}
	}
	
	void FixedUpdate() 
	{
		RaycastHit hit;
		
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
		}		
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
