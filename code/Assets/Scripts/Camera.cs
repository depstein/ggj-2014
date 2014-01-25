using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	
	Vector2 direction;
	Quaternion rotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find ("Player");

		direction = transform.position + ((player.transform.position + 5 * (player.transform.rotation * Vector3.down)) - transform.position) / 2;
		rotation = player.transform.rotation * Quaternion.Euler(330, 0, 0);
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(direction.x, direction.y, transform.position.z);
		transform.rotation = rotation;
	}
}
