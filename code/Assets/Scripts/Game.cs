using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public double health;
	public static Game game;

	void Awake() {
		game = this;
		health = 30f;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		health -= Time.deltaTime;

		if (health <= 0) {
			Debug.Log ("ded");
		}
	}
}
