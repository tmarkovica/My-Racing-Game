using UnityEngine;

public class RotateWheels : MonoBehaviour
{
	[SerializeField]
	private Rigidbody carRigidBodyComponentRef;
	
	[SerializeField]
	private Transform leftFrontWheel, rightFrontWheel, leftRearWheel, rightRearWheel;
	
	void FixedUpdate() 
	{
		float dotProduct = Vector3.Dot(carRigidBodyComponentRef.velocity, carRigidBodyComponentRef.gameObject.transform.forward);
				
		if (dotProduct > 1 || dotProduct < -1) 
		{
			SpinWheel(leftFrontWheel, dotProduct / 1.5f);
			SpinWheel(rightFrontWheel, dotProduct / 1.5f);
			SpinWheel(leftRearWheel, dotProduct / 1.5f);
			SpinWheel(rightRearWheel, dotProduct / 1.5f);
		}
	}
	
	void SpinWheel(Transform wheelTransform, float rotationAmount)
	{
		wheelTransform.Rotate(Vector3.right, rotationAmount);
	}
}
