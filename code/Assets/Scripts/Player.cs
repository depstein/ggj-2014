using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(10, 10);
	private Vector2 direction;
	private Vector3 mouseDirection;
	private Vector3 pupilPosition;
	private Quaternion new_direction;
	public GameObject whatIFire;
	public GameObject hat;
	public Transform hatParent;

	private Animator _animator; 
	private GameObject _back;
	private GameObject _front;
	private GameObject _pupil;
	private Transform _mouth;

	private Vector3 _projectileDirection;

	// Use this for initialization
	void Start () 
	{
		var hatObj = (GameObject)Instantiate (hat);
		hatObj.transform.parent = hatParent;
		hatObj.transform.localPosition = hat.transform.localPosition;
		hatObj.transform.localRotation = hat.transform.localRotation;

		_animator = GetComponent<Animator>();

		_back = transform.Find("body/back").gameObject;
		_front = transform.Find("body/front").gameObject;
		_pupil = transform.Find ("body/front/pupil").gameObject;
		_mouth = transform.Find("body/front/mouth");

		ShowFront();
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetMovementDirection();
		CheckFire();
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
			var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.1f * mouseDirection, transform.rotation);
			projectile.rigidbody2D.velocity = 20 * mouseDirection;
			projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, mouseDirection);
			_projectileDirection = mouseDirection;
			_animator.SetTrigger("Attack");
		}
	}

	void DoFire()
	{
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * _projectileDirection, transform.rotation);
		projectile.rigidbody2D.velocity = 20 * _projectileDirection;
	}

	private void ShowBack()
	{
		_back.SetActive(true);
		_front.SetActive(false);
	}

	private void ShowFront()
	{
		_front.SetActive(true);
		_back.SetActive(false);
	}
}
