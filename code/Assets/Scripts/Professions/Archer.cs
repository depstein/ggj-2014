using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Archer : Profession {
	
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private float cast_expire = 0;
	private float cast_expire_length = 2f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		_mouth = transform.Find("body/front/mouth");

		whatIFire = (GameObject)Resources.Load("Arrow", typeof(GameObject));
	}

	private bool drawing = false;
	private List<Vector3> points;
	private Vector3 curve;
	private bool do_curve = false;
	
	// Update is called once per frame
	void Update () {
		if (cast_expire < 0) {
			do_curve = false;
		}

		if (cast_expire > 0)
		{
			cast_expire -= Time.deltaTime;
		}

		if (Input.GetMouseButton (1)) 
		{

			if(!drawing)
			{
				drawing = true;
				points = new List<Vector3>();
			}
		}

		if(drawing) 
		{
			var mouse_position = Mouse.position;
			if (points.Count == 0 || Vector3.Distance(points[points.Count - 1], mouse_position) >= 0.5f)
				points.Add(mouse_position);

			if(!Input.GetMouseButton (1))
			{
				drawing = false;

				var direction = points[points.Count - 1] - points[0];
				var unit_direction = direction.normalized;

				bool matches_line = true;
				for(int i = 0; i < points.Count - 1; i++)
				{
					if(Vector3.Dot(unit_direction, (points[i + 1] - points[i]).normalized) < 0.7f)
					{
						matches_line = false;
						break;
					}
				}

				if(matches_line)
				{
					curve = direction;
					do_curve = true;
					cast_expire = cast_expire_length;
				}

				points = null;
			}
		}

		CheckFire();
	}

	void CheckFire()
	{
		if (Input.GetMouseButtonDown (0)) {
			_projectileDirection = _player.mouseDirection.normalized;
			_animator.SetTrigger("Attack");
		}
	}

	void DoFire()
	{
		Vector3 offset = new Vector3(0.12f * _projectileDirection.x, 0.12f * _projectileDirection.y, _animatedCharacter.facingForward ? 0 : 1);
		var projectile = (GameObject)Instantiate (whatIFire, _mouth.position + 0.5f * _projectileDirection, transform.rotation);
		projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, _projectileDirection);
		projectile.rigidbody2D.velocity = 30f * _projectileDirection;

		if (do_curve)
		{
			projectile.GetComponent<Projectile> ().curve = curve;
			do_curve = false;
		}

		Game.game.PlayerFired ();
	}
}
