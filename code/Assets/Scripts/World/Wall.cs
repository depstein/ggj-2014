using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wall
{
	public static GameObject WallTemplate;
	public static GameObject TreeTemplate;
	public static GameObject AreaDetectorTemplate;
	public static Material DebugMaterialTemplate;
	public static bool EnableDebug = false;
	public static float tree_radius = 2.0f;

	public static GameObject CreateSolid(Vector3 start, Vector3 end)
	{
		var delta = end - start;
		var position = start + delta / 2;
		var scale = new Vector3 (1, delta.magnitude, 1);
		var rotate = Quaternion.FromToRotation (Vector3.up, delta);
		
		var wall = GameObject.Instantiate (WallTemplate, position, rotate) as GameObject;
		wall.transform.localScale = scale;
		
		return wall;
	}

	private static void CreateTree(Vector3 position)
	{
		position = new Vector3 (position.x, position.y, position.y);
		GameObject.Instantiate (TreeTemplate, position, Quaternion.identity);
	}

	private static void CreateDetector(Vector3 start, Vector3 end)
	{
		var delta = end - start;
		var position = start + delta / 2;
		var scale = new Vector3 (1, delta.magnitude, 1);
		var rotate = Quaternion.FromToRotation (Vector3.up, delta);
		
		var wall = GameObject.Instantiate (AreaDetectorTemplate, position, rotate) as GameObject;
		wall.transform.localScale = scale;
	}

	public static GameObject Create(Vector3 start, Vector3 end)
	{
		if (end.y < start.y) 
		{
			var tmp = start;
			start = end;
			end = tmp;
		}
		var distance = Vector3.Distance(start, end);
		var steps = Mathf.CeilToInt (distance / tree_radius);
		var direction = (end - start).normalized;

		var current = start;
		for (int i = 0; i <= steps; i++) 
		{
			CreateTree(current);

			current += (distance / steps) * direction;
		}

		return null;
	}

	public static GameObject CreateDebug(Vector3 start, Vector3 end)
	{
		if (!EnableDebug) return null;

		var wall = CreateSolid (start, end);
		wall.transform.localScale = new Vector3(0.1f, wall.transform.localScale.y, wall.transform.localScale.z);
		wall.transform.position = new Vector3 (wall.transform.position.x, wall.transform.position.y, 20);
		wall.GetComponent<MeshRenderer> ().materials = new Material[] { DebugMaterialTemplate };
		GameObject.Destroy (wall.GetComponent<BoxCollider2D>());
		
		return wall;
	}
}
