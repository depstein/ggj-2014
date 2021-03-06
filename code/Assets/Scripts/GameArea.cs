﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameArea {

	public GameObject gameAreaTarget;
	public List<GameObject> goodObjects;
	public List<GameObject> badObjects;
	public IArea myArea;
	private float spawnSheepFor = 1f;

	public GameArea(IArea area) {
		myArea = area;
		goodObjects = new List<GameObject> ();
		badObjects = new List<GameObject> ();

		gameAreaTarget = GameObject.Instantiate (Resources.Load<GameObject>("Pen"), area.GetOriginLocation(), Quaternion.identity) as GameObject;
		AreaObject areaObj = gameAreaTarget.GetComponent<AreaObject>();
		areaObj.gameArea = this;

		for (int i=0; i<Random.Range (7,10); i++) {
			GameObject bad = GameObject.Instantiate (Resources.Load<GameObject>("Rabbit"), area.GetSpawnLocation(), Quaternion.identity) as GameObject;
			areaObj = bad.GetComponent<AreaObject>();
			areaObj.gameArea = this;
			AddBadObject(bad);
		}

		for (int i=0; i<Random.Range (2,4); i++) {
			GameObject bad = GameObject.Instantiate (Resources.Load<GameObject>("Sheep"), area.GetSpawnLocation(), Quaternion.identity) as GameObject;
			areaObj = bad.GetComponent<AreaObject>();
			areaObj.gameArea = this;
			AddBadObject(bad);
		}

		area.PlayerEntered += delegate(Vector3 position) {
					Player.player.gameArea = this;
					this.spawnSheepFor = 3f;
				};
		area.PlayerExisted += delegate(Vector3 position) {
			if(Player.player.gameArea == this)
			{
				Player.player.gameArea = null;
			}
				};
	}

	public void playerHere() {
		spawnSheepFor -= Time.deltaTime/30f;
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

	public void SpawnX(string name)
	{
		GameObject obj = GameObject.Instantiate (Resources.Load<GameObject>(name), myArea.GetSpawnLocation(), Quaternion.identity) as GameObject;
		var area_obj = obj.GetComponent<AreaObject>();
		area_obj.gameArea = this;
		AddBadObject(obj);
	}
	
	public void SpawnEnemy() { SpawnX ("Enemy"); }
	public void SpawnRabbit() { SpawnX ("Rabbit"); }
	public void SpawnSheep() {
				if (Random.value < spawnSheepFor) {
						SpawnX ("Sheep");
				}
		}

	public void TurnGoodThingsTo(GameObject goodThing)
	{
		List<GameObject> newGood = new List<GameObject> ();
		foreach (GameObject obj in goodObjects) {
			GameObject good = GameObject.Instantiate (goodThing) as GameObject;
			good.GetComponent<AreaObject>().gameArea = obj.GetComponent<AreaObject>().gameArea;
			good.transform.position = obj.transform.position;
			good.transform.rotation = obj.transform.rotation;
			newGood.Add (good);
			GameObject.Destroy (obj);
		}

		goodObjects.AddRange (newGood);
	}

	public void TurnBadThingsTo(GameObject badThing)
	{
		List<GameObject> newBad = new List<GameObject> ();
		foreach (GameObject obj in badObjects) {
			GameObject bad = GameObject.Instantiate (badThing) as GameObject;
			bad.GetComponent<AreaObject>().gameArea = obj.GetComponent<AreaObject>().gameArea;
			bad.transform.position = obj.transform.position;
			bad.transform.rotation = obj.transform.rotation;
			newBad.Add (bad);
			GameObject.Destroy (obj);
		}

		badObjects.AddRange (newBad);
	}
}
