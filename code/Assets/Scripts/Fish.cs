using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

	bool sitting = false;
	float nextTime = -1;

	Vector3 target;
	Vector3 velocity;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nextTime < 0 || nextTime <= Time.time) {
			if (sitting)
			{
				target = transform.position;
				nextTime = Time.time + (0.75f + Random.value * 0.5f);
			}
			else
			{
				var direction = Random.onUnitSphere;
				direction.z = 0;
				direction.Normalize();
				direction *= 3;
				target = transform.position + direction;
				nextTime = Time.time + (0.5f + Random.value * 0.25f);
			}
			sitting = !sitting;
		}

		velocity = (target - transform.position);
	}



	void FixedUpdate()
	{
		rigidbody2D.velocity = velocity * (nextTime - Time.time) / 0.5f;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Colidiu");
		Destroy (this.gameObject);
	}
}
