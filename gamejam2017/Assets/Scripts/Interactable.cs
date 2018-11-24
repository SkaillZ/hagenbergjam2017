using UnityEngine;

public class Interactable : MonoBehaviour
{
	public InteractionEvent firstInteractionEvent;
	
	public void Interact()
	{
		StartCoroutine(firstInteractionEvent.Execute());
	}

	private void OnDrawGizmos()
	{
		if (GetComponent<BoxCollider2D>() == null)
		{
			return;
		}
		Gizmos.color = new Color(0f, 1f, 0f, 0.6f);
		Gizmos.DrawCube(transform.position + (Vector3) GetComponent<BoxCollider2D>().offset, GetComponent<BoxCollider2D>().size);
	}
}
