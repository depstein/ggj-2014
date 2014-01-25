using UnityEngine;
using System.Collections;

public class Archer : MonoBehaviour {

	private Animator _animator;
	private Player _player;
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_player = GetComponent <Player> ();
		_mouth = transform.Find("body/front/mouth");

		_player.PutOnHat(Resources.Load("Archer Hat"));
	}
	
	// Update is called once per frame
	void Update () {
		CheckFire();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 offset = new Vector3(0.12f * _player.mouseDirection.x, 0.12f * _player.mouseDirection.y, _player.facingForward ? 0 : 1);
			var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + offset, transform.rotation);
			
			projectile.rigidbody2D.velocity = 20 * _player.mouseDirection;
			projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, _player.mouseDirection);
			_projectileDirection = _player.mouseDirection;
			_animator.SetTrigger("Attack");
		}
	}
	
	void DoFire()
	{
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * _projectileDirection, transform.rotation);
		projectile.rigidbody2D.velocity = 20 * _projectileDirection;
	}
}
