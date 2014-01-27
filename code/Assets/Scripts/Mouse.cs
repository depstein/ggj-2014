using UnityEngine;
using System.Collections;

public class Mouse
{
	public static Vector3 position {
		get {
			var camera = Camera.camcorder.gameObject.camera;
			return camera.ScreenToWorldPoint (new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		}
	}
}