using UnityEngine;
using System.Collections;

public class Sheep : MonoBehaviour {

	bool sitting = false;
	bool isInRegion = false;
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
		if (isInRegion) {
			velocity = Vector3.zero;
			return;
		}

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

	public void RunFromPlayer(Transform playerLocation)
	{	
		var direction = - (playerLocation.position - transform.position);
		direction.z = 0;
		direction.Normalize();
		direction *= Random.Range(8,10);
		Debug.Log ("RUN AWAY!!!: " + direction);
		target = transform.position + direction;
		nextTime = Time.time + (0.5f + Random.value * 0.25f);
		sitting = true;
	}

	void FixedUpdate()
	{
		rigidbody2D.velocity = velocity * (nextTime - Time.time) / 0.5f;
	}
}
