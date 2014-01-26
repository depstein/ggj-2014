using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameArea {

	public GameObject gameAreaTarget;
	public List<GameObject> goodObjects;
	public List<GameObject> badObjects;
	public IArea myArea;

	public GameArea(IArea area) {
		myArea = area;
		goodObjects = new List<GameObject> ();
		badObjects = new List<GameObject> ();

		gameAreaTarget = GameObject.Instantiate (Resources.Load<GameObject>("Objective"), area.GetOriginLocation(), Quaternion.identity) as GameObject;
		AreaObject areaObj = gameAreaTarget.GetComponent<AreaObject>();
		areaObj.gameArea = this;

		for (int i=0; i<Random.Range (8,10); i++) {
			GameObject bad = GameObject.Instantiate (Resources.Load<GameObject>("BadObject"), area.GetSpawnLocation(), Quaternion.identity) as GameObject;
			areaObj = bad.GetComponent<AreaObject>();
			areaObj.gameArea = this;
			AddBadObject(bad);
		}

		for (int i=0; i<Random.Range (8,10); i++) {
			GameObject good = GameObject.Instantiate (Resources.Load<GameObject>("GoodObject"), area.GetSpawnLocation(), Quaternion.identity) as GameObject;
			areaObj = good.GetComponent<AreaObject>();
			areaObj.gameArea = this;
			AddGoodObject(good);
		}

		area.PlayerEntered += delegate(Vector3 position) {
			Player.player.gameArea = this;
				};
		area.PlayerExisted += delegate(Vector3 position) {
			if(Player.player.gameArea == this)
			{
				Player.player.gameArea = null;
			}
				};
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

	public void AddRandomHat()
	{
		GameObject.Instantiate (Resources.Load<GameObject>(Hat.RandomHat()), myArea.GetSpawnLocation(), Quaternion.identity);
	}
}
