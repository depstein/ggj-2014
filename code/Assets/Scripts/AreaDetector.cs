using UnityEngine;
using System.Collections;

public class AreaDetector : MonoBehaviour {

	public event PlayerEnteredDelegate PlayerEntered;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject == Player.player) {
			Debug.Log("Player hit me");
			PlayerEntered(Player.player.transform.position);
		}
	}
}
