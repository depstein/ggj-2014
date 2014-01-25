using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	Vector2 direction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject player = GameObject.Find ("Player");

		direction = transform.position + (player.transform.position - transform.position) / 2;
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(direction.x, direction.y, transform.position.z);
	}
}
