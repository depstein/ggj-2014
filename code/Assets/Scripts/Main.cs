using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour 
{
	public GameObject PlayerTemplate;
	public GameObject WallTemplate;
	public GameObject TreeTemplate;
	public GameObject GrassTemplate;
	public Material DebugMaterialTemplate;
	
	void Awake()
	{
		Wall.WallTemplate = WallTemplate;
		Wall.TreeTemplate = TreeTemplate;
		WorldManager.GrassTemplate = GrassTemplate;
		Wall.DebugMaterialTemplate = DebugMaterialTemplate;
		WorldManager.worldManager = new WorldManager ();
	}
	
	void Start () 
	{
		WorldManager.worldManager.Load ();
		var starting_area = WorldManager.worldManager.PickRandomArea ();

		Player.player = (Instantiate (PlayerTemplate, starting_area.position, Quaternion.identity) as GameObject).GetComponent<Player>();
		Camera.camcorder = (GameObject.Find ("Camera") as GameObject).GetComponent<Camera>();
	}
}
