using UnityEngine;

public class CollisionTryout : MonoBehaviour
{
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
	
	/* void OnTriggerEnter(Collider collider)
	{
		Debug.Log(collider.bounds);
		
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
		}
	} */
	
	void OnTriggerEnter(Collider collider)
	{
		if (collider.CompareTag("Wall") == true)
		{			
			Debug.Log(collider.name);
		}
	}
}
