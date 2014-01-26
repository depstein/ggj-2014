using UnityEngine;
using System.Collections;

public class Pen : MonoBehaviour {

	void Awake() {
		//TODO: remove when we stop using MainScene.
		GameArea.gameArea = new GameArea (null);
		GameArea.gameArea.gameAreaTarget = this.gameObject;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
