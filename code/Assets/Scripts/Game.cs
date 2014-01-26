using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public float health;
	private float hatTimer;
	public static Game game;
	public const float SPAWN_ENEMY_EVERY = 10f;
	public float difficulty = 0.0f;
	public float enemyTimer;

	void Awake() {
		game = this;
		health = 30f;
		enemyTimer = SPAWN_ENEMY_EVERY;
	}

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI () {
		GUI.Box (new Rect (10f, 50f, health*4f, 20f), ""+((int)(health)));
	}

	// Update is called once per frame
	void Update () {
		health -= Time.deltaTime;
		enemyTimer -= Time.deltaTime;

		if (enemyTimer < 0) {
			enemyTimer = SPAWN_ENEMY_EVERY * (1f - difficulty);
			if (Player.player.gameArea != null)
				Player.player.gameArea.SpawnEnemy();
		}

		if (health <= 0) {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#elif UNITY_WEBPLAYER
			Application.OpenURL(webplayerQuitURL);
			#else
			Application.Quit();
			#endif
		}
	}
}
