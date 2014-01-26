using UnityEngine;
using System.Collections;

public class AreaDetector : MonoBehaviour {

	public delegate void PlayerHitDelegate(Vector3 position);
	public event PlayerHitDelegate PlayerHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private bool m_inside = false;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player>() == Player.player) {
			if (!m_inside)
			{
				m_inside = true;
				Debug.Log("Player hit me");
				if (PlayerHit != null)
					PlayerHit(Player.player.transform.position);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.GetComponent<Player> () == Player.player) {
			if (m_inside)
			{
				m_inside = false;
			}
		}
	}
}
