using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(10, 10);
	private Vector2 direction;
	public Vector3 mouseDirection;
	private Vector3 pupilPosition;
	private Quaternion new_direction;
	public Transform hatParent;

	private GameObject _pupil;


	private Vector3 _projectileDirection;

	private Animator _animator; 
	private AnimatedCharacter _animatedCharacter; 

	// Use this for initialization
	void Start () 
	{
		_animator = GetComponent<Animator>();

		_pupil = transform.Find ("body/front/pupil").gameObject;

		gameObject.AddComponent<Archer>();
		GetComponent<Archer>().whatIFire = (GameObject)Resources.Load("archer-projectile", typeof(GameObject));

		_animatedCharacter = GetComponent<AnimatedCharacter>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetMovementDirection();
	}

	void FixedUpdate()
	{
		var old_direction = rigidbody2D.velocity;

		var diff = direction - old_direction;

		rigidbody2D.AddForce (diff);
		rigidbody2D.velocity = direction - diff / 2;
	}

	public void PutOnHat(Object hatObj) {
		GameObject hat = (GameObject)Instantiate (hatObj);
		hat.transform.parent = hatParent;
	}

	void SetMovementDirection()
	{
		var horizontal = Input.GetAxis ("Horizontal") * speed.x;
		var vertical = Input.GetAxis ("Vertical") * speed.y;

		direction = new Vector2 (horizontal, vertical);
		
		var camera = GameObject.Find("Main Camera").camera;
		
		var screen = camera.WorldToScreenPoint (transform.position);
		
		var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen.z);
		
		mouseDirection = mouse - screen;
		mouseDirection.Normalize ();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
			_projectileDirection = mouseDirection;
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
