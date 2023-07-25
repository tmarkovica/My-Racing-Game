using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
	void OnGUI()
	{
		this.GetComponent<Text>().text = ((int)(1.0f / Time.smoothDeltaTime)).ToString();
	}
}
