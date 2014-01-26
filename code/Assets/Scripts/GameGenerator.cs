using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator : MonoBehaviour 
{
	
	public GameObject WallTemplate;
	public Material DebugMaterialTemplate;

	void Start () 
	{
		Wall.WallTemplate = WallTemplate;
		Wall.DebugMaterialTemplate = DebugMaterialTemplate;
		var manager = new LevelManager ();
	}

	void Update () 
	{
	
	}
}
