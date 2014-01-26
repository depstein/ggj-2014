using UnityEngine;
using System.Collections;

public class Pen : AreaObject {

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
