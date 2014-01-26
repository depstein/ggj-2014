using UnityEngine;
using System.Collections;

public class Hat : AreaObject {
	public static readonly string[] HAT_TYPES = {"ArcherHat", "ShepherdStaff"};
	public const float SURVIVAL_TIME = 15f;
	private float timeAlive;

	public static string RandomHat()
	{
		return HAT_TYPES [(int)(Random.value * HAT_TYPES.Length)];
	}

	// Use this for initialization
	void Start () {
		timeAlive = SURVIVAL_TIME;
	}
	
	// Update is called once per frame
	void Update () {
		timeAlive -= Time.deltaTime;

		if (timeAlive <= 0) {
			Destroy(this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Player> () != null) {
			if(other.gameObject.GetComponent<Profession>() != null) {
				Destroy(other.gameObject.GetComponent<Profession>());
			}
			switch(this.gameObject.name) { //TODO: this will fail when we start cloning things (aka making them dynamically)
				case "ArcherHat":
					WorldMode.ChangeModeTo(other.gameObject, PlayerMode.archer, gameArea);
				    break;
				case "ShepherdStaff":
				WorldMode.ChangeModeTo(other.gameObject, PlayerMode.shepherd, gameArea);
					break;
			}
			Destroy (this.gameObject);
		}
	}
}
