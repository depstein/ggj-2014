using UnityEngine;
using System.Collections;

public class Hat : MonoBehaviour {
	private bool connectedToPlayer = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			Debug.Log (this.gameObject.name);
			//other.gameObject.GetComponent<Player>().PutOnHat();
			Destroy (this.gameObject);
		}
	}
}
