using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public static GameObject player;
	Vector2 direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) return;
		
		direction = transform.position + ((player.transform.position + 2 * (player.transform.rotation * Vector3.up) - transform.position) / 2);
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(direction.x, direction.y, transform.position.z);
	}
}
