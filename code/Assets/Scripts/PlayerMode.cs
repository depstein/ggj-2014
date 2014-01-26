using UnityEngine;
using System.Collections;

public enum PlayerMode {
	normal, archer, shepherd
}

public class WorldMode {
	private static PlayerMode mode = PlayerMode.normal;
	public static PlayerMode Mode { get { return mode; } }

	public static void ChangeModeTo(GameObject player, PlayerMode newMode, GameArea gameArea) {
		switch (newMode) {
		case PlayerMode.normal:

			break;
		case PlayerMode.archer:
			player.AddComponent<Archer>();
			gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("Quiver"));
			gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
			break;
		case PlayerMode.shepherd:
			player.AddComponent<Shepherd>();
			gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("Sheep"));
			gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Water"));
			break;
		}
		mode = newMode;
	}
}
