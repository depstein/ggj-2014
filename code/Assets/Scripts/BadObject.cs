using UnityEngine;
using System.Collections;

public class BadObject : AreaObject {

	public bool sitting = false;
	public float nextTime = -1;
	
	public Vector3 target;
	public Vector3 velocity;

	void OnDestroy() {
		gameArea.RemoveBadObject (this.gameObject);
		gameArea.RemoveGoodObject (this.gameObject);
	}

	public virtual bool ShouldCheckForMove()
	{
		return true;		
	}

	public void RunFromPlayer(Transform playerLocation)
	{
		var direction = - (playerLocation.position - transform.position);
		direction.z = 0;
		direction.Normalize();
		direction *= Random.Range(8,10);
		target = transform.position + direction;
		nextTime = Time.time + (0.5f + Random.value * 0.25f);
		sitting = true;
	}
	
	void FixedUpdate()
	{
		rigidbody2D.velocity = velocity * (nextTime - Time.time) / 0.5f;
	}

	// Update is called once per frame
	public virtual void Update () 
	{
		if (ShouldCheckForMove() && (nextTime < 0 || nextTime <= Time.time)) {
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

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Projectile> () != null) {
			Destroy (this.gameObject);
			Destroy (other.gameObject);
		}
	}
}
