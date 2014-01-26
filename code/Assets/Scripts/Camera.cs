using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {
	public static Camera camera;


	private Vector2 direction;

	void Update ()
	{
		direction = transform.position + ((Player.player.transform.position + 2 * (Player.player.transform.rotation * Vector3.up) - transform.position) / 2);
	}

	void FixedUpdate()
	{
		transform.position = new Vector3(direction.x, direction.y, transform.position.z);
	}
}
