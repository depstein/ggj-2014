using UnityEngine;
using System.Collections;

public class Player : AreaObject {
	public static Player player;

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

		gameObject.AddComponent<Shepherd> ();
		gameObject.AddComponent<Archer> ();


		GameObject hatObj = (GameObject)Resources.Load ("WizardHat");
		GameObject beardObj = (GameObject)Resources.Load ("ShepherdBeard");
		GameObject handObj = (GameObject)Resources.Load ("ShepherdStaff");

		_hat = (GameObject)Instantiate (hatObj);
		_hat.transform.parent = hatParent;
		_hat.transform.localPosition = hatObj.transform.localPosition;
		_hat.transform.localRotation = hatObj.transform.localRotation;
		_beard = (GameObject)Instantiate (beardObj);
		_beard.transform.parent = beardParent;
		_beard.transform.localPosition = beardObj.transform.localPosition;
		_beard.transform.localRotation = beardObj.transform.localRotation;
		_hand = (GameObject)Instantiate (handObj);
		_hand.transform.parent = handParent;
		_hand.transform.localPosition = handObj.transform.localPosition;
		_hand.transform.localRotation = handObj.transform.localRotation;

		renderer.sortingLayerName = "Foreground";
	}
	
	// Update is called once per frame
	void Update () 
	{
		SetMovementDirection();

		if (gameArea != null) {
			gameArea.playerHere();
		}
	}

	void FixedUpdate()
	{
		var old_direction = rigidbody2D.velocity;

		var diff = direction - old_direction;

		rigidbody2D.AddForce (diff);
		rigidbody2D.velocity = direction - diff / 2;
	}

	void SetMovementDirection()
	{
		var horizontal = Input.GetAxis ("Horizontal") * speed.x;
		var vertical = Input.GetAxis ("Vertical") * speed.y;

		direction = new Vector2 (horizontal, vertical);

		mouseDirection = (Mouse.position - transform.position).normalized;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Projectile> () != null) {
			Game.game.health -= 1;
			Destroy (other.gameObject);
		}
	}
}
