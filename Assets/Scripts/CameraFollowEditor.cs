using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameHandler))]
public class CameraFollowEditor : Editor
{
	private int defaultFollow = -1;
	
	public override void OnInspectorGUI()
	{
		GameHandler gh = (GameHandler)target;
		
		DrawDefaultInspector();
		
		if (GUILayout.Button("Camera follow player"))
		{
			gh.SetCameraToFollow(-1);
		}
		
		int oponentsCount = gh.OponentsCount();
		for (int i = 0; i < oponentsCount; i++)
		{
			if (GUILayout.Button("Camera follow oponent " + i))
			{
				gh.SetCameraToFollow(i);
			}			
		}
		
	}
}