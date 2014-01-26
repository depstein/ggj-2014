﻿using UnityEngine;
using System.Collections;

public class Archer : Profession {
	
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private int ammoCount = 0;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		_mouth = transform.Find("body/front/mouth");

		_player.PutOnHat(Resources.Load<GameObject>("ArcherHat"));
		whatIFire = (GameObject)Resources.Load("Arrow", typeof(GameObject));
	}
	
	// Update is called once per frame
	void Update () {
		CheckFire();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0) && ammoCount > 0) {
			ammoCount--;
			_projectileDirection = _player.mouseDirection;
			_animator.SetTrigger("Attack");
		}
	}

	void DoFire()
	{
		Vector3 offset = new Vector3(0.12f * _projectileDirection.x, 0.12f * _projectileDirection.y, _animatedCharacter.facingForward ? 0 : 1);
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * _projectileDirection, transform.rotation);
		projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, _projectileDirection);
		projectile.rigidbody2D.velocity = 20 * _projectileDirection;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Quiver>() != null) {
			Destroy (other.gameObject);
			ammoCount++; //TODO: should we put more than one arrow in the quiver?
		}
	}
}
