using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	public float health;
	private float hatTimer;
	public static Game game;

	void Awake() {
		game = this;
		health = 30f;
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
