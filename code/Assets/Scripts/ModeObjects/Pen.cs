using UnityEngine;
using System.Collections;

public class Pen : AreaObject {

	void OnTriggerEnter2D(Collider2D other)
	{
		Sheep s = other.gameObject.GetComponent<Sheep> ();
		if (s != null && !s.isInRegion && s.gameArea == Player.player.gameArea) {
			UnityEngine.Behaviour b = s.GetComponent("Halo") as UnityEngine.Behaviour;
			b.enabled = true;
			Game.game.points++;
			s.isInRegion = true;
		}
	}
}
