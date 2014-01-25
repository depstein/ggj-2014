using UnityEngine;
using System.Collections;

public class GameArea : MonoBehaviour {
	
	public static GameArea gameArea;
	public GameObject gameAreaTarget;

	void Awake() {
		gameArea = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
