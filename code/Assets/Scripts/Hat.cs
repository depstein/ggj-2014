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
			Destroy(other.gameObject.GetComponent<Profession>());
			switch(this.gameObject.name) { //TODO: this will fail when we start cloning things (aka making them dynamically)
				case "Archer Hat":
					other.gameObject.AddComponent<Archer>();
					GameArea.gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("archer-good-pickup"));
					GameArea.gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
				    break;
				case "shepherd-staff":
					other.gameObject.AddComponent<Shepherd>();
					GameArea.gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("sheep"));
					//GameArea.gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
					break;
			}
			Destroy (this.gameObject);
		}
	}
}
