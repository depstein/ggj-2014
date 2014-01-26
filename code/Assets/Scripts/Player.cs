using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 speed = new Vector2(10, 10);
	private Vector2 direction;
	public Vector3 mouseDirection;
	private Vector3 pupilPosition;
	private Quaternion new_direction;
	public Transform hatParent;
	public Transform handParent;
	public Transform beardParent;

	private GameObject _pupil;
	private GameObject _hat;
	private GameObject _beard;
	private GameObject _hand;


	private Vector3 _projectileDirection;

	private Animator _animator; 

	// Use this for initialization
	void Start () 
	{
		_animator = GetComponent<Animator>();

		_pupil = transform.Find ("body/front/pupil").gameObject;
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

	public void PutOnHat(GameObject hatObj, GameObject beardObj = null, GameObject handObj = null) {
		if (_hat != null) {
			Destroy (_hat);
		}
		if (_beard != null) {
			Destroy (_beard);
		}
		if (_hand != null) {
			Destroy (_hand);
		}
		if (hatObj != null) {
			_hat = (GameObject)Instantiate (hatObj);
			if (_hat.collider2D != null) {
				_hat.collider2D.enabled = false;
			}
			_hat.transform.parent = hatParent;
			_hat.transform.localPosition = hatObj.transform.localPosition;
			_hat.transform.localRotation = hatObj.transform.localRotation;
		}
		if (beardObj != null) {
			_beard = (GameObject)Instantiate (beardObj);
			if(_beard.collider2D != null) {
				_beard.collider2D.enabled = false;
			}
			_beard.transform.parent = beardParent;
			_beard.transform.localPosition = beardObj.transform.localPosition;
			_beard.transform.localRotation = beardObj.transform.localRotation;
		}
		if (handObj != null) {
			_hand = (GameObject)Instantiate (handObj);
			if(_hand.collider2D != null) {
				_hand.collider2D.enabled = false;
			}
			_hand.transform.parent = handParent;
			_hand.transform.localPosition = handObj.transform.localPosition;
			_hand.transform.localRotation = handObj.transform.localRotation;
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
}
