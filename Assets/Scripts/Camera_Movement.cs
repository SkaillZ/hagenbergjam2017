using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour {

	public GameObject player;
	public Transform leftDelimiter;
	public Transform rightDelimiter;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}

	void LateUpdate ()
	{
		if (player.transform.position.x > leftDelimiter.transform.position.x && player.transform.position.x < rightDelimiter.transform.position.x) 
		{
			transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
		}
	}
}