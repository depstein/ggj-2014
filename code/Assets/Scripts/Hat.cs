using UnityEngine;
using System.Collections;

public class Hat : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			switch(this.gameObject.name) {
				case "Archer Hat":
					other.gameObject.AddComponent<Archer>();
					break;
			}
			Destroy (this.gameObject);
		}
	}
}
