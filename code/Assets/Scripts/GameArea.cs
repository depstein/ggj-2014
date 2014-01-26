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

	public void AttachToArea(IArea area)
	{
		// TODO: implement
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
		goodObjects.Remove (objectToBeAdded);
	}
	
	public void RemoveBadObject(GameObject objectToBeAdded)
	{
		badObjects.Remove (objectToBeAdded);
	}

	public void TurnGoodThingsTo(GameObject goodThing)
	{
		List<GameObject> newGood = new List<GameObject> ();
		foreach (GameObject obj in goodObjects) {
			GameObject good = (GameObject)Instantiate (goodThing);
			good.transform.position = obj.transform.position;
			good.transform.rotation = obj.transform.rotation;
			Destroy (obj);
		}
	}

	public void TurnBadThingsTo(GameObject badThing)
	{
		List<GameObject> newBad = new List<GameObject> ();
		foreach (GameObject obj in badObjects) {
			GameObject bad = (GameObject)Instantiate (badThing);
			bad.transform.position = obj.transform.position;
			bad.transform.rotation = obj.transform.rotation;
			Destroy (obj);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}
