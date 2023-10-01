using UnityEditor;
using UnityEngine;

public class FinishWall : MonoBehaviour
{
	[SerializeField] public int totalLaps;
	
	public InGameMenu inGameMenu;
	
	private int lapCount_player = -1;
	private int lapCount_oponent = -1;
	
	private void OnTriggerEnter(Collider collider) 
	{
		if (collider.transform.parent.gameObject.name == "Player") 
		{
			lapCount_player++;
			
			if (lapCount_player == totalLaps) 
			{
				FinishRace();
			}
		}
		else if (collider.transform.parent.gameObject.name == "Oponents") 
		{
			lapCount_oponent++;
		}	
	}
	
	private void FinishRace() 
	{
		EditorUtility.audioMasterMute = true;
		
		if (lapCount_player > lapCount_oponent) 
		{
			EditorUtility.DisplayDialog("You have WON the race!", "Go To Car Selection", "Ok");
			inGameMenu.CarSelection();
		}
		else 
		{
			bool result = EditorUtility.DisplayDialog("You have LOST the race!", "Go To Car Selection or restart race", "Ok", "Restart");
			if (result)
				inGameMenu.CarSelection();
			else
				inGameMenu.RestartGame();
		}
	}
}
