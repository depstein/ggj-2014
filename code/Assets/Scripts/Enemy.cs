﻿using UnityEngine;
using System.Collections;

public class Enemy : BadObject {
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	const float FIRE_FREQUENCY = 5f;
	private float fireTime = FIRE_FREQUENCY;

	private Animator _animator;
	protected AnimatedCharacter _animatedCharacter;

	// Use this for initialization
	protected void Start () {
		_animator = GetComponent<Animator>();
		_animatedCharacter = GetComponent<AnimatedCharacter>();

		_mouth = transform.Find("body/front/mouth");
		
		whatIFire = (GameObject)Resources.Load("EnemyArrow", typeof(GameObject));
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update ();
		fireTime -= Time.deltaTime;

		if (fireTime <= 0) {
			fireTime = FIRE_FREQUENCY + Random.Range(-2, 2);
			_projectileDirection = Player.player.gameObject.transform.position - gameObject.transform.position;
			_projectileDirection.Normalize();
			_animator.SetTrigger("Attack");
		}
	}
	
	void DoFire()
	{
		Vector3 offset = new Vector3(0.12f * _projectileDirection.x, 0.12f * _projectileDirection.y, _animatedCharacter.facingForward ? 0 : 1);
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * offset, transform.rotation);
		projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, _projectileDirection);
		projectile.rigidbody2D.velocity = 20 * _projectileDirection;
	}
}
