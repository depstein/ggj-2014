using UnityEngine;
using System.Collections;

public class Shepherd : Profession {
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();

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
				Debug.Log ("      "+obj.name + " running");
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
				}
	}
}