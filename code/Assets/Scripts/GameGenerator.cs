using UnityEngine;
using System.Collections;

public class Wall
{
	public static GameObject WallTemplate;
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
}


public class PathNode
{
	public float width = 0.0f;
	public Vector3 position;
	public Vector3 direction;
}

public class Path
{
	static float max_width = 20.0f;
	static float min_width = 5.0f;
	static float max_width_variance = 1.5f;
	static float min_width_variance = 0.5f;
	
	static float max_turn = 60.0f;
	static float min_turn = 1.0f;
	
	static float max_edge_length = 25.0f;
	static float min_edge_length = 5.0f;

	private ArrayList m_points = new ArrayList();

	public Vector3 MakeNewDirection(Vector3 direction)
	{
		var rotation = Quaternion.AngleAxis(min_turn + ((Random.value - 0.5f) * (max_turn - min_turn) * 2.0f), Vector3.back);
		return (rotation * direction).normalized * Random.Range (min_edge_length, max_edge_length);
	}

	public Path(Vector3 begin, Vector3 direction, float width)
	{
		for (int i = 0; i < 20; i++) {
			var node = new PathNode() { 
				width = width,
				position = begin,
				direction = direction
			};

			m_points.Add (node);

			var new_direction = MakeNewDirection(direction);
			var new_width = Mathf.Min (Mathf.Max(Random.Range(min_width_variance, max_width_variance) * width), min_width, max_width);
			var new_begin = begin + new_direction;

			begin = new_begin;
			direction = new_direction;
			width = new_width;
		}
		
		//if (width_current > 8.0f && Random.value > 0.6f)
		//{
		//	Instantiate(Fish, (start + (end - start) / 2), Quaternion.AngleAxis(Random.value * 180, Vector3.forward));
		//}
	}

	public void CreateEdge(PathNode previous, PathNode current)
	{
		var start_right = previous.width * Vector3.Cross (previous.direction, Vector3.forward).normalized;
		var start_left = previous.width * Vector3.Cross (previous.direction, Vector3.back).normalized;
		
		var end_right = current.width * Vector3.Cross (current.direction, Vector3.forward).normalized;
		var end_left = current.width * Vector3.Cross (current.direction, Vector3.back).normalized;
		
		Wall.Create (previous.position + start_right, current.position + end_right);
		Wall.Create (previous.position + start_left, current.position + end_left);
	}

	public void CreateWalls()
	{
		for(int i = 0; i < m_points.Count; i++)
		{
			var current = m_points[i] as PathNode;
			PathNode previous;
			if (i == 0)
			{
				previous = new PathNode() { 
					direction = current.direction, 
					position = current.position - current.direction,
					width = current.width
				};
			}
			else
			{
				previous = m_points[i - 1] as PathNode;
			}

			CreateEdge(previous, current);
		}
	}
}

public class MissingWall
{
	public Vector3 start;
	public Vector3 end;
}

public class AreaEntry
{
	public float angle = 0.0f;
	public float radius = 0.0f;
	public bool create = true;
}

public class Area
{
	public static float max_radius = 50.0f;
	public static float min_radius = 20.0f;
	public static float max_radius_variance = 1.5f;
	public static float min_radius_variance = 0.5f;
	
	public static float max_angle_variance = 3f;
	public static float min_angle_variance = 0.5f;

	public static int min_points = 8;
	public static int max_points = 30;

	public static float total_angle = 360f;

	public Vector3 m_position;
	public int m_points;
	public ArrayList m_entries = new ArrayList();

	float AverageRemainingAngle(int current_point, float angle_used)
	{
		var remaining_angle = total_angle - angle_used;
		var remaining_points = m_points - current_point;

		return remaining_angle / remaining_points;
	}

	public MissingWall GetWall()
	{
		int wall = Random.Range (0, m_entries.Count - 1);
		(m_entries [wall] as AreaEntry).create = false;
		return new MissingWall ()
		{
			start = NodePosition(wall),
			end = NodePosition(wall + 1)
		};
	}

	public Area(Vector3 position)
	{
		m_position = position;
		m_points = min_points + (int)(Random.value * (max_points - min_points));

		float old_radius = min_radius + (max_radius - min_radius) / 2;
		float angle_used = 0f;
		for (int i = 0; i < m_points; i++) {
			float average_remaining = AverageRemainingAngle(i, angle_used);

			var area_entry = new AreaEntry() { 
				radius = Mathf.Min (Mathf.Max(Random.Range(min_radius_variance, max_radius_variance) * old_radius, min_radius), max_radius),
				angle = angle_used + Random.Range(min_angle_variance, max_angle_variance) * average_remaining
			};

			//Debug.Log(string.Format("Entry {0}: {1} {2}", m_entries.Count, area_entry.radius, area_entry.angle));

			m_entries.Add(area_entry);

			old_radius = area_entry.radius;
			angle_used = area_entry.angle;
		}
		//Debug.Log(string.Format("Total Angle Used: {0}", angle_used));
	}

	private AreaEntry At(int i)
	{
		if (i == m_entries.Count)
			i = 0;

		return m_entries [i] as AreaEntry;
	}

	private Vector3 NodePosition(int i)
	{
		var area_entry = At (i);
		var unit_direction = Quaternion.AngleAxis(area_entry.angle, Vector3.back) * Vector3.up;
		var direction = area_entry.radius * unit_direction;

		//Debug.Log (string.Format ("({0}, {1}, {2}", direction.x, direction.y, direction.z));
		return m_position + direction;
	}

	public void CreateWalls()
	{
		for(int i = 0; i < m_entries.Count; i++)
		{
			if (!At (i).create)
				continue;

			Wall.Create(NodePosition(i), NodePosition(i + 1));
		}
	}

}

public class AreaManager
{

}

public class GameGenerator : MonoBehaviour {
	
	public GameObject WallTemplate;
	public GameObject Fish;


	// Use this for initialization
	void Start () {
		Wall.WallTemplate = WallTemplate;

		var area = new Area (Vector3.zero);

		var wall = area.GetWall ();

		var direction = wall.end - wall.start;
		var p = new Path(wall.start + direction / 2, Vector3.Cross(direction.normalized, Vector3.back), direction.magnitude / 2);

		area.CreateWalls ();
		p.CreateWalls ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
