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

	void Shout()
	{
		foreach (GameObject obj in _player.gameArea.goodObjects) {
			Vector3 diff = obj.transform.position - transform.position;
			if (diff.magnitude < 5)
			{
				Sheep s = obj.GetComponent<Sheep>();
				if (s != null)
				{
					s.RunFromPlayer(transform);
				}
			}
		}
	}
}