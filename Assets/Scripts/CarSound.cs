using UnityEngine;

public class CarSound : MonoBehaviour
{
	public AudioSource src;
	
	public AudioClip ler, speedingUp, speedingDown, speedingUp_low, speedingDown_low;
		
	void Start()
	{
		src.clip = ler;
		src.loop = true;		
		src.Play();
	}

	void Update()
	{
		float input = Input.GetAxis("Vertical");

		if (input > 0f)
		{
			if (input > 0.5f)
				PlayThisSoundIfClipStopped(speedingUp);
			else 
				PlayThisSoundIfClipStopped(speedingUp_low);
		}
		else if (input < 0f)
		{
			PlayThisSoundIfClipStopped(speedingDown);
		}
		else
		{
			PlayThisSoundIfClipStopped(ler);
		}
	}
	
	private void PlayThisSoundIfClipStopped(AudioClip clip) 
	{
		this.src.clip = clip;
		if (!src.isPlaying)
			src.Play();
	}
}
