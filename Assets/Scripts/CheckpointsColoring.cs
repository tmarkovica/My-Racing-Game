using UnityEngine;

public class CheckpointsColoring : MonoBehaviour
{
	[SerializeField] Transform checkpoints;
	[SerializeField] Material normal;
	[SerializeField] Material highlighted;
	
	public static CheckpointsColoring Instance;
	
	void Awake() 
	{
		Instance = this;
	}
	
	void Start()
	{
		for (int i = 0; i < checkpoints.childCount; i++)
			checkpoints.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
	}
	
	public void ToggleCheckpointHighlight(Transform checkpoint, bool highlight) 
	{
		checkpoint.GetComponent<MeshRenderer>().material = highlight ? highlighted : normal;
	}
}
