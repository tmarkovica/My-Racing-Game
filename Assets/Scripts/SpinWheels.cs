using UnityEngine;

public class SpinWheels : MonoBehaviour
{
	private Rigidbody carRigidBodyRef;
	private Transform[] wheels = new Transform[4];
	
	void Start() 
	{
		carRigidBodyRef = this.GetComponent<Rigidbody>();
		wheels[0] = this.transform.Find("Front_Left_Wheel");
		wheels[1] = this.transform.Find("Front_Right_Wheel");
		wheels[2] = this.transform.Find("Rear_Left_Wheel");
		wheels[3] = this.transform.Find("Rear_Right_Wheel");
	}
	
	void FixedUpdate() 
	{
		float dotProduct = Vector3.Dot(carRigidBodyRef.velocity, carRigidBodyRef.transform.forward);
				
		if (IsCarMoving(dotProduct))
		{
			foreach (Transform wheel in wheels)
				SpinWheel(wheel, dotProduct / 1.5f);
		}
	}
	
	private bool IsCarMoving(float dotProduct) { return (dotProduct > 1) || (dotProduct < -1); }
	
	void SpinWheel(Transform wheelTransform, float rotationAmount)
	{
		wheelTransform.Rotate(Vector3.right, rotationAmount);
	}
}
