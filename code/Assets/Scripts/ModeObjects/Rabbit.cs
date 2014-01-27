using UnityEngine;
using System.Collections;

public class Rabbit : BadObject {
	// Use this for initialization
	void Start () {
		distanceToMove = 10;
		timeWaitScale = 1.5f;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update ();
	}
}
