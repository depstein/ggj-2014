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
			if(other.gameObject.GetComponent<Profession>() != null) {
				Destroy(other.gameObject.GetComponent<Profession>());
			}
			switch(this.gameObject.name) { //TODO: this will fail when we start cloning things (aka making them dynamically)
				case "Archer Hat":
					WorldMode.ChangeModeTo(other.gameObject, PlayerMode.archer);
				    break;
				case "shepherd-staff":
				WorldMode.ChangeModeTo(other.gameObject, PlayerMode.shepherd);
					break;
			}
			Destroy (this.gameObject);
		}
	}
}
