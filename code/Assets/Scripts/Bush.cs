using UnityEngine;
using System.Collections;

public class Bush : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Rabbit> () != null) {
			Debug.Log ("FISH");
			Game.game.health -= 2f;
		}
	}
}
