using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall
{
	public static GameObject WallTemplate;
	public static Material DebugMaterialTemplate;
	public static GameObject Create(Vector3 start, Vector3 end)
	{
		var delta = end - start;
		var position = start + delta / 2;
		var scale = new Vector3 (1, delta.magnitude, 1);
		var rotate = Quaternion.FromToRotation (Vector3.up, delta);
		
		var wall = GameObject.Instantiate (WallTemplate, position, rotate) as GameObject;
		wall.transform.localScale = scale;
		
		return wall;
	}
	public static GameObject CreateDebug(Vector3 start, Vector3 end)
	{
		var wall = Create (start, end);
		wall.transform.localScale = new Vector3(0.1f, wall.transform.localScale.y, wall.transform.localScale.z);
		wall.transform.position = new Vector3 (wall.transform.position.x, wall.transform.position.y, 20);
		wall.GetComponent<MeshRenderer> ().materials = new Material[] { DebugMaterialTemplate };
		GameObject.Destroy (wall.GetComponent<BoxCollider2D>());
		
		return wall;
	}
}
