using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DG.Tweening;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {
	public void StopSound()
	{
		transform.parent.GetComponent<AudioSource>().DOFade(0f, 8f);

	}
	
	public void PlaySound()
	{
		GetComponent<AudioSource>().Play();
	}
}
