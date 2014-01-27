using UnityEngine;
using System.Collections;

public class Projectile: MonoBehaviour {

	public GameObject Bullet;
	private float timeLeft = 1f;
	public Vector3 curve;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) 
		{
			Destroy(this.gameObject);
		}

		transform.rotation = Quaternion.FromToRotation (Vector3.up, rigidbody2D.velocity);
	}

	void FixedUpdate()
	{
		if (curve.x != 0 && curve.y != 0)
		{
			var angle = Quaternion.FromToRotation(rigidbody2D.velocity, curve);
			angle = Quaternion.RotateTowards(Quaternion.identity, angle, curve.magnitude / 4);
			curve = angle * curve;
			rigidbody2D.velocity = angle * rigidbody2D.velocity;
		}
	}
}
