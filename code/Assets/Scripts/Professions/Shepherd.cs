using UnityEngine;
using System.Collections;

public class Shepherd : Profession {
	
	private Animator _animator;
	private Player _player;
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private AnimatedCharacter _animatedCharacter; 
	
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_player = GetComponent <Player> ();
		_mouth = transform.Find("body/front/mouth");
		
		_player.PutOnHat(Resources.Load<GameObject>("Archer Hat"));
		_animatedCharacter = GetComponent<AnimatedCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
		CheckShout();
	}

	void CheckShout() {
		if(Input.GetKeyDown(KeyCode.Space))
		{
			Shout ();
		}
	}

	void Shout()
	{
		foreach (GameObject obj in GameArea.gameArea.goodObjects) {
			Vector3 diff = obj.transform.position - transform.position;
			if (diff.magnitude < 5)
			{
				Sheep s = obj.GetComponent<Sheep>();
				if (s != null)
				{
					s.RunFromPlayer(transform);
				}
			}
		}
	}
}