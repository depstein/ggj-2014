using UnityEngine;
using System.Collections;

public class Rabbit : BadObject {
	Vector3 target;
	Vector3 velocity;
	// Use this for initialization
	void Start () {
		base.Start ();
		target = GameArea.gameArea.gameAreaTarget.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target - transform.position;

		direction.z = 0;
		direction.Normalize();

		velocity = direction * 200;
	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = velocity * Time.deltaTime;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Projectile> () != null) {
			Destroy (this.gameObject);
			Destroy (other.gameObject);
		}
	}
}
