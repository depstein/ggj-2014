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

	// Use this for initialization
	void Start () 
	{
		_animator = GetComponent<Animator>();

		_pupil = transform.Find ("body/front/pupil").gameObject;

		Camera.player = gameObject;

		//gameObject.AddComponent<Shepherd>();
		//GetComponent<Archer>().whatIFire = (GameObject)Resources.Load("archer-projectile", typeof(GameObject));
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

	public void PutOnHat(GameObject hatObj) {
		GameObject hat = (GameObject)Instantiate (hatObj);
		hat.transform.parent = hatParent;
		hat.transform.localPosition = hatObj.transform.localPosition;
		hat.transform.localRotation = hatObj.transform.localRotation;
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
