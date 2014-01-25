using UnityEngine;
using System.Collections;

public class GameGenerator : MonoBehaviour {
	
	public GameObject Wall;
	public GameObject Fish;

	void CreateWall(Vector3 start, Vector3 end)
	{
		var delta = end - start;
		var position = start + delta / 2;
		var scale = new Vector3 (1, delta.magnitude, 1);
		var rotate = Quaternion.FromToRotation (Vector3.up, delta);

		var w1 = Instantiate (Wall, position, rotate) as GameObject;
		w1.transform.localScale = scale;

	}

	void Path(Vector3 previous, float width_previous, Vector3 start, Vector3 end, float width)
	{
		var direction_previous = start - previous;
		var direction_current = end - start;
		
		var start_a = width_previous * Vector3.Cross (direction_previous, Vector3.forward).normalized;
		var start_b = width_previous * Vector3.Cross (direction_previous, Vector3.back).normalized;
		
		var end_a = width * Vector3.Cross (direction_current, Vector3.forward).normalized;
		var end_b = width * Vector3.Cross (direction_current, Vector3.back).normalized;
		
		CreateWall (start + start_a, end + end_a);
		CreateWall (start + start_b, end + end_b);
	}

	Vector3 MakeNewDirection(Vector3 direction)
	{
		var new_direction = Quaternion.AngleAxis((Random.value - 0.5f) * 50, Vector3.forward) * direction;
		new_direction.Normalize();
		new_direction *= (5 + 10 * Random.value);
		return new_direction;
	}

	void WritePath()
	{

	}

	// Use this for initialization
	void Start () {

		float minimum_width = 5.0f;
		float maximum_change = 4.0f;
		
		float width_previous = 10f;
		float width_current = 10f;
		Vector3 previous = new Vector3 (0, -1);
		Vector3 start = new Vector3 ();
		Vector3 end = new Vector3 (0, 5);

		for (int i = 0; i < 50; i++) {
			Path (previous, width_previous, start, end, width_current);

			if (width_current > 8.0f && Random.value > 0.6f)
			{
				Instantiate(Fish, (start + (end - start) / 2), Quaternion.AngleAxis(Random.value * 180, Vector3.forward));
			}

			var new_direction = MakeNewDirection(end - start);
			var new_width = Mathf.Max(width_current + (Random.value - 0.5f) * maximum_change, minimum_width);

			width_previous = width_current;
			width_current = new_width;
			previous = start;
			start = end;
			end = start + new_direction;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
