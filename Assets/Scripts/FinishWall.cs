using UnityEngine;
using UnityEngine.UI;

public class FinishWall : MonoBehaviour
{
	[SerializeField] public int totalLaps;

	[SerializeField] private GameObject finishDialog;
    [SerializeField] private Text dialogTextRaceFinished;

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
		AudioListener.pause = true;
		Time.timeScale = 0;
		
		if (lapCount_player > lapCount_oponent) 
		{
			dialogTextRaceFinished.text = "You WON!";
		}
		else 
		{
            dialogTextRaceFinished.text = "You LOST!";
        }
        finishDialog.SetActive(true);
	}
}
