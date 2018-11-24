using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float moveSpeed = 3f;

	public bool isInteracting;
	public bool isWarping;
	public bool dead;
	private List<Interactable> intersectingInteractables = new List<Interactable>();

	private Animator anim;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		GameManager.Instance.player = this;
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void FixedUpdate() {
		
		if (isInteracting || isWarping || dead)
		{
			anim.SetFloat("Horizontal", 0);
			return;
		}

		float axis = Input.GetAxis("Horizontal");

		if (GameManager.Instance.IsFlagSet("WeedConsume"))
		{
			axis *= 0.5f;
		}
		if (GameManager.Instance.IsFlagSet("CarryCorpse1"))
		{
			axis *= 0.5f;
		}
		if (GameManager.Instance.IsFlagSet("CarryCorpse2"))
		{
			axis *= 0.5f;
		}
		
		if (GameManager.Instance.IsFlagSet("ConsumeSpeed"))
		{
			axis *= 2;
		}
		
		anim.SetFloat("Horizontal", Math.Abs(axis));
		
		float horizontal = axis * moveSpeed * Time.deltaTime;
		var pos = new Vector2(transform.position.x + horizontal, transform.position.y);
		GetComponent<Rigidbody2D> ().MovePosition (pos);
		if (Input.GetButtonDown("Interact") && intersectingInteractables.Count > 0)
		{
			intersectingInteractables[0].Interact();
		}

		if (horizontal < -0.01)
		{
			spriteRenderer.flipX = true;
		}
		if (horizontal > 0.01)
		{
			spriteRenderer.flipX = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("enter " + other);

		var interactable = other.GetComponent<Interactable>();
		if (interactable == null)
			return;
		
		intersectingInteractables.Add(interactable);
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//Debug.Log("exit" + other);
		var interactable = other.GetComponent<Interactable>();
		if (interactable == null)
			return;
		
		intersectingInteractables.Remove(interactable);
	}

	public void Die()
	{
		anim.SetTrigger("Die");
		dead = true;
	}

	public void PlayDeathSound()
	{
		GetComponent<AudioSource>().Play();
	}
}
