using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameArea : MonoBehaviour {
	
	public static GameArea gameArea;
	public GameObject gameAreaTarget;
	public List<GameObject> goodObjects;
	public List<GameObject> badObjects;

	void Awake() {
		gameArea = this;
		goodObjects = new List<GameObject> ();
		badObjects = new List<GameObject> ();
	}

	public void AddGoodObject(GameObject objectToBeAdded)
	{
		goodObjects.Add (objectToBeAdded);
	}

	public void AddBadObject(GameObject objectToBeAdded)
	{
		badObjects.Add (objectToBeAdded);
	}

	public void RemoveGoodObject(GameObject objectToBeAdded)
	{
		goodObjects.Add (objectToBeAdded);
	}
	
	public void RemoveBadObject(GameObject objectToBeAdded)
	{
		badObjects.Add (objectToBeAdded);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
