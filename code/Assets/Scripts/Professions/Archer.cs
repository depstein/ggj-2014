using UnityEngine;
using System.Collections;

public class Archer : Profession {
	
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private bool fired = false;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		_mouth = transform.Find("body/front/mouth");

		whatIFire = (GameObject)Resources.Load("Arrow", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () {
		CheckFire();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
			fired = true;
			_projectileDirection = _player.mouseDirection;
			_animator.SetTrigger("Attack");
		}
	}

	void DoFire()
	{
		if (fired) {
					fired = false;
					Vector3 offset = new Vector3 (0.01f * _projectileDirection.x, 0.01f * _projectileDirection.y, _animatedCharacter.facingForward ? 0 : 1);
					var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + offset, transform.rotation);
					projectile.transform.rotation = Quaternion.FromToRotation (Vector3.up, _projectileDirection);
					projectile.rigidbody2D.velocity = 20 * _projectileDirection;

					Game.game.PlayerFired ();
				}
	}
}
