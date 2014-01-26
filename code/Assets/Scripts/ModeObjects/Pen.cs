using UnityEngine;
using System.Collections;

public class Pen : MonoBehaviour {

	void Awake() {
		//TODO: remove when we stop using MainScene.
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("COLLIDED");
		Sheep s = other.gameObject.GetComponent<Sheep> ();
		if (s != null) {
			Debug.Log ("COLLIDED SHEEP");
			s.isInRegion = true;
		}
	}
}
