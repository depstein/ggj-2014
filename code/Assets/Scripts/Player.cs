using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(10, 10);
	private Vector2 direction;
	public Vector3 mouseDirection;
	private Vector3 pupilPosition;
	private Quaternion new_direction;
	public Transform hatParent;

	private Animator _animator;
	private GameObject _back;
	private GameObject _front;
	private GameObject _pupil;

	public bool facingForward = true;

	// Use this for initialization
	void Start () 
	{
		_animator = GetComponent<Animator>();

		_back = transform.Find("body/back").gameObject;
		_front = transform.Find("body/front").gameObject;
		_pupil = transform.Find ("body/front/pupil").gameObject;

		gameObject.AddComponent<Archer>();
		GetComponent<Archer>().whatIFire = (GameObject)Resources.Load("archer-projectile", typeof(GameObject));

		ShowFront();
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

		_animator.SetFloat("SpeedSqr", rigidbody2D.velocity.sqrMagnitude);
		//transform.rotation = new_direction;

		if (direction.y > 0)
		{
			ShowBack();
		}
		if (direction.y < 0)
		{
			ShowFront();
		}
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
		Vector3 offset = new Vector3(0.12f * _projectileDirection.x, 0.12f * _projectileDirection.y, facingForward ? 0 : 1);
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * _projectileDirection, transform.rotation);
		projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, _projectileDirection);
		projectile.rigidbody2D.velocity = 20 * _projectileDirection;
	}

	private void ShowBack()
	{
		_back.SetActive(true);
		_front.SetActive(false);
		facingForward = false;
	}

	private void ShowFront()
	{
		_front.SetActive(true);
		_back.SetActive(false);
		facingForward = true;
	}
}
