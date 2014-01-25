﻿using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {

	private Animator _animator;
	private Player _player;
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private AnimatedCharacter _animatedCharacter; 

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_player = GetComponent <Player> ();
		_mouth = transform.Find("body/front/mouth");

		_player.PutOnHat(Resources.Load<GameObject>("Archer Hat"));
		_animatedCharacter = GetComponent<AnimatedCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckFire();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
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
}
