using UnityEngine;
using System.Collections;

public class BadObject : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameArea.gameArea.AddGoodObject (this.gameObject);
	}

}
