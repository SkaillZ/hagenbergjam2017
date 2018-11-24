using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarpWithCameraSwapInteraction : InteractionEvent
{
	[Header("Fade")]
	public Transform targetPosition;
	public Camera newCamera;
	
	public Image fadeImage;
	public float fadeSpeed = 0.02f;

	public InteractionEvent next;

	protected override IEnumerator DoExecute()
	{
		var player = GameManager.Instance.player;
		player.isWarping = true;
		var playerBody = player.GetComponent<Rigidbody2D>();
		var prevConstraints = playerBody.constraints;

		float fade = 0f;
		while (fade < 1f)
		{
			fade += fadeSpeed;
			fadeImage.color = new Color(0f, 0f, 0f, fade);
			yield return null;
		}
		
		playerBody.constraints = RigidbodyConstraints2D.None;
		playerBody.position = targetPosition.position;
		player.transform.position = targetPosition.position;
		playerBody.constraints = prevConstraints;
		player.isWarping = false;
		Camera.main.gameObject.SetActive(false);
		newCamera.gameObject.SetActive(true);
		
		yield return new WaitForSecondsRealtime(0.2f);
		

		while (fade > 0f)
		{
			fade -= fadeSpeed;
			fadeImage.color = new Color(0f, 0f, 0f, fade);
			yield return null;
		}
	}
}
