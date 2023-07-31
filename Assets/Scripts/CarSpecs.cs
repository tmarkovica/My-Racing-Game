using UnityEngine;

public class CarSpecs : MonoBehaviour
{
	[SerializeField] public float maxSpeed = 700f;
	[SerializeField] public float accelerationTime = 5f;
	[SerializeField] public float breakingStrength = 2.5f;
	[SerializeField] public float turnStrength = 2f;
	public const float maxWheelTurn_deg = 50f;
}