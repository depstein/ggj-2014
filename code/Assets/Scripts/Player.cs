using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(10, 10);
	private Vector2 direction;
	private Quaternion new_direction;
	public GameObject whatIFire;
	public GameObject hat;
	public Transform hatParent;

	private Animator _animator; 
	private GameObject _back;
	private GameObject _front;

	// Use this for initialization
	void Start () 
	{
		var hatObj = (GameObject)Instantiate (hat);
		hatObj.transform.parent = hatParent;
		hatObj.transform.localPosition = Vector3.zero;
		hatObj.transform.localRotation = Quaternion.identity;

		_animator = GetComponent<Animator>();

		_back = transform.Find("body/back").gameObject;
		_front = transform.Find("body/front").gameObject;

		ShowFront();
	}
	
	// Update is called once per frame
	void Update () 
	{
		RotatePlayer();
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

	void RotatePlayer()
	{
		var horizontal = Input.GetAxis ("Horizontal") * speed.x;
		var vertical = Input.GetAxis ("Vertical") * speed.y;
		
		var camera = GameObject.Find("Main Camera").camera;
		
		var screen = camera.WorldToScreenPoint (transform.position);
		
		var mouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screen.z);
		
		var desired_direction = mouse - screen;
		//var current_direction = transform.rotation * Vector3.one;
		
		var desired_rotation = Quaternion.FromToRotation (Vector3.up, desired_direction);
		
		new_direction = Quaternion.RotateTowards(transform.rotation, desired_rotation, Quaternion.Angle(transform.rotation, desired_rotation) / 8);
		
		direction = new Vector2 (horizontal, vertical);
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
			var oneDirection =  transform.rotation * Vector3.up;
			var projectile = (GameObject)Instantiate (whatIFire, transform.position + 0.5f * oneDirection, transform.rotation);
			projectile.rigidbody2D.velocity = 20 * oneDirection;
		}
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
