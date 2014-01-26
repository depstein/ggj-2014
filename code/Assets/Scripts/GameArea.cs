using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameArea {
	public GameObject gameAreaTarget;
	public List<GameObject> goodObjects;
	public List<GameObject> badObjects;

	public GameArea(IArea area) {
		Debug.Log ("GameAreaMade");
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
			GameObject good = GameObject.Instantiate (goodThing) as GameObject;
			good.transform.position = obj.transform.position;
			good.transform.rotation = obj.transform.rotation;
			GameObject.Destroy (obj);
		}
	}

	public void TurnBadThingsTo(GameObject badThing)
	{
		List<GameObject> newBad = new List<GameObject> ();
		foreach (GameObject obj in badObjects) {
			GameObject bad = GameObject.Instantiate (badThing) as GameObject;
			bad.transform.position = obj.transform.position;
			bad.transform.rotation = obj.transform.rotation;
			GameObject.Destroy (obj);
		}
	}
}
