using UnityEngine;
using System.Collections;

public class Hat : AreaObject {

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			if(other.gameObject.GetComponent<Profession>() != null) {
				Destroy(other.gameObject.GetComponent<Profession>());
			}
			switch(this.gameObject.name) { //TODO: this will fail when we start cloning things (aka making them dynamically)
				case "Archer Hat":
					other.gameObject.AddComponent<Archer>();
					gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("archer-good-pickup"));
					gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
				    break;
				case "shepherd-staff":
					other.gameObject.AddComponent<Shepherd>();
					gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("sheep"));
					//GameArea.gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
					break;
			}
			Destroy (this.gameObject);
		}
	}
}
