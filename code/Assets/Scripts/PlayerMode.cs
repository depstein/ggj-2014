using UnityEngine;
using System.Collections;

public enum PlayerMode {
	normal, archer, shepherd
}

public class WorldMode {
	private static PlayerMode mode;
	public static PlayerMode Mode { get { return mode; } }

	public static void ChangeModeTo(GameObject player, PlayerMode newMode) {
		switch (newMode) {
		case PlayerMode.normal:

			break;
		case PlayerMode.archer:
			player.AddComponent<Archer>();
			GameArea.gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("archer-good-pickup"));
			GameArea.gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
			break;
		case PlayerMode.shepherd:
			player.AddComponent<Shepherd>();
			GameArea.gameArea.TurnGoodThingsTo(Resources.Load<GameObject>("sheep"));
			//GameArea.gameArea.TurnBadThingsTo(Resources.Load<GameObject>("Rabbit"));
			break;
		}
		mode = newMode;
	}
}
