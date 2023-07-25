using UnityEngine;

public class KeepVehicleGrounded : MonoBehaviour
{
	private Rigidbody rigidBody;
	private BoxCollider boxCollider;
	
	[SerializeField] private bool loggingEnabled = false;
	
	void Awake()
	{
		rigidBody = this.GetComponent<Rigidbody>();
		boxCollider = this.GetComponent<BoxCollider>();

	}
	
	void FixedUpdate()
	{
		if (!GroundTerrainData.Instance.IsGrounded(rigidBody.transform.position, boxCollider.size.y * this.transform.localScale.y / 2))
		{
			rigidBody.AddForce(Vector3.down * 200000f, ForceMode.Force);
			
			if (loggingEnabled) Debug.Log("not grounded");
		}
		else if (loggingEnabled) Debug.Log("grounded");
	}
}