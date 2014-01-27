using UnityEngine;
using System.Collections;

public class Shepherd : Profession {
	GameObject whatIFire;
	private Transform _mouth;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();

		_mouth = transform.Find("body/front/mouth");

		whatIFire = (GameObject)Resources.Load("Shout", typeof(GameObject));
		_animator.SetTrigger("Attack");
	}
	
	// Update is called once per frame
	void Update () {
		CheckShout();
	}

	void CheckShout() {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Shout ();
		}
	}

	void ShoutAt(GameObject obj)
	{
		Vector3 diff = obj.transform.position - transform.position;
		if (diff.magnitude < 5)
		{
			BadObject s = obj.GetComponent<BadObject>();
			if (s != null)
			{
				s.RunFromPlayer(transform);
			}
		}
	}
	void Shout()
	{
		if (_player.gameArea != null) {
						foreach (GameObject obj in _player.gameArea.badObjects) {
								ShoutAt (obj);
						}

						foreach (GameObject obj in _player.gameArea.goodObjects) {
								ShoutAt (obj);
						}
						
						_animator.SetTrigger("Attack");
						Instantiate (whatIFire, _mouth.position, transform.rotation);
				}
	}
}