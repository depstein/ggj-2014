using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface ITimerControl
{
	void SetTimer(float new_time);
}

public delegate void TimerFireDelegate ();
public class Timers
{
	public ITimerControl Add(float time, TimerFireDelegate fire)
	{
		var timer = new Timer () { fire = fire, time = time, time_since_fire = 0 };
		m_timers.Add(timer);
		return timer;
	}

	public void Update()
	{
		foreach (Timer i in m_timers) {
			i.time_since_fire += Time.deltaTime;
			i.Check();
		}
	}

	private class Timer : ITimerControl
	{
		public TimerFireDelegate fire;
		public float time_since_fire;
		public float time;

		public void Check()
		{
			if (time_since_fire >= time)
			{
				time_since_fire = 0;
				fire();
			}
		}

		public void SetTimer(float new_time)
		{
			time = new_time;
			Check ();
		}
	}

	private List<Timer> m_timers = new List<Timer>();
}

public class Game : MonoBehaviour {

	public static Game game;
	public const float SPAWN_ENEMY_EVERY = 10f;
	public const float SPAWN_RABBIT_EVERY = 10f;
	public const float SPAWN_SHEEP_EVERY = 8f;

	public int health;
	public int points;
	public float difficulty = 0.0f;
	
	public ITimerControl enemyTimer;
	public ITimerControl rabbitTimer;
	public ITimerControl sheepTimer;
	public ITimerControl reduceDifficultyTimer;
	public ITimerControl pointTimer;

	public Timers timers = new Timers();

	void Awake() { game = this; health = 60; }

	public void ChangeDifficulty(float byAmt)
	{
		SetDifficulty (difficulty += byAmt);
	}

	void SetDifficulty(float new_difficulty)
	{
		new_difficulty = Mathf.Max (Mathf.Min (new_difficulty, 1f), 0);
		enemyTimer.SetTimer (SPAWN_ENEMY_EVERY * (1.3f + (1f - new_difficulty) / 2) + Random.Range (-0.2f * SPAWN_ENEMY_EVERY, 0.2f * SPAWN_ENEMY_EVERY));
		rabbitTimer.SetTimer (SPAWN_RABBIT_EVERY * (0.8f + (new_difficulty / 2)) + Random.Range (-0.2f * SPAWN_RABBIT_EVERY, 0.2f * SPAWN_RABBIT_EVERY));
		difficulty = new_difficulty;
	}

	public void PlayerFired()
	{
		SetDifficulty (difficulty + 0.02f);
	}

	// Use this for initialization
	void Start () {
		enemyTimer = timers.Add (SPAWN_ENEMY_EVERY + Random.Range (-0.1f * SPAWN_ENEMY_EVERY, 0.1f * SPAWN_ENEMY_EVERY), delegate() { if (Player.player.gameArea != null) Player.player.gameArea.SpawnEnemy (); });
		rabbitTimer = timers.Add (SPAWN_RABBIT_EVERY + Random.Range (-0.1f * SPAWN_RABBIT_EVERY, 0.1f * SPAWN_RABBIT_EVERY), delegate() { if (Player.player.gameArea != null) Player.player.gameArea.SpawnRabbit (); });
		sheepTimer = timers.Add (SPAWN_SHEEP_EVERY + Random.Range (-0.1f * SPAWN_SHEEP_EVERY, 0.1f * SPAWN_SHEEP_EVERY), delegate() { if (Player.player.gameArea != null) Player.player.gameArea.SpawnSheep (); });
		reduceDifficultyTimer = timers.Add (1f, delegate() {
						SetDifficulty (difficulty - .001f);
			health -= 1;
			points++;
						if (health <= 0)
			{
				Lost ();
			}
				});

		SetDifficulty (0f);
	}

	void OnGUI () {
		GUI.TextField (new Rect (10f, 50f, 50f, 15f), "Health:", GUIStyle.none);
		GUI.Box (new Rect (60f, 50f, 15f + health * 4f, 15f), "");
		GUI.TextField (new Rect (10f, 80f, 70f, 20f), "Violence Level:", GUIStyle.none);
		GUI.Box (new Rect (100f, 80f, 15f + difficulty * 120f, 15f), "");
		GUI.TextField (new Rect (10f, 110, 70f, 20f), "Time alive: "+points + " seconds", GUIStyle.none);
	}

	void Lost()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
		#else
		Application.Quit();
		#endif
	}

	// Update is called once per frame
	void Update () {
		timers.Update ();
	}
}
