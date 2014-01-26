using UnityEngine;
using System.Collections;

public class GoodObject : MonoBehaviour {
	// Use this for initialization
	void Start () {
		GameArea.gameArea.AddGoodObject (this.gameObject);
	}
}
