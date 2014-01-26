﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator : MonoBehaviour 
{
	public GameObject PlayerTemplate;
	public GameObject WallTemplate;
	public Material DebugMaterialTemplate;
	
	private GameObject player_object;
	private Player player;

	void Start () 
	{
		Wall.WallTemplate = WallTemplate;
		Wall.DebugMaterialTemplate = DebugMaterialTemplate;
		var manager = new LevelManager ();

		var area = manager.PickRandomArea ();

		player_object = Instantiate (PlayerTemplate, area.position, Quaternion.identity) as GameObject;
		player = player_object.GetComponent<Player> ();

		var camera = GameObject.Find ("Main Camera") as GameObject;

		var player_position = player_object.transform.position;
		camera.transform.position = new Vector3(player_position.x, player_position.y, camera.transform.position.z);
	}

	void Update () 
	{
	
	}
}
