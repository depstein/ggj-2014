using UnityEngine;
using System.Collections;

public class Chef : MonoBehaviour {

	private Animator _animator;
	private Player _player;
	private Transform _mouth;
	public GameObject whatIFire;
	private Vector3 _projectileDirection;
	private AnimatedCharacter _animatedCharacter; 
	private int[] ingredientCount;
	public static int numIngredients = 3;

	void Awake() {
		ingredientCount = new int[numIngredients];
	}
	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator>();
		_player = GetComponent <Player> ();
		_mouth = transform.Find("body/front/mouth");

		_animatedCharacter = GetComponent<AnimatedCharacter>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void AddIngredient(int ingredientType)
	{
		ingredientCount [ingredientType]++;
	}
}
