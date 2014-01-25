using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(50, 50);
	private Vector2 direction;
	private Quaternion new_direction;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		var horizontal = Input.GetAxis ("Horizontal") * speed.x;
		var vertical = Input.GetAxis ("Vertical") * speed.y;

		var camera = GameObject.Find("Main Camera").camera;

		var screen = camera.WorldToScreenPoint (transform.position);

		var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen.z);

		var desired_direction = mouse - screen;
		//var current_direction = transform.rotation * Vector3.one;

		var desired_rotation = Quaternion.FromToRotation (Vector3.up, desired_direction);

		new_direction = Quaternion.RotateTowards(transform.rotation, desired_rotation, Quaternion.Angle(transform.rotation, desired_rotation) / 8);

		direction = new Vector2 (horizontal, vertical);
	}

	void FixedUpdate()
	{
		var old_direction = rigidbody2D.velocity;

		var diff = direction - old_direction;

		rigidbody2D.AddForce (diff);
		rigidbody2D.velocity = direction - diff / 2;

		transform.rotation = new_direction;
	}

	
}
