using UnityEngine;

[CreateAssetMenu(fileName = "CarSpecs", menuName = "CarSpecifications Scriptable Object")]
public class CarSpecifications : ScriptableObject
{
	public float maxSpeed = 700f;
	public float accelerationTime = 5f;
	public float breakingStrength = 2.5f;
	public float turnStrength = 2f;
	public float maxWheelTurn_deg = 50f;
}