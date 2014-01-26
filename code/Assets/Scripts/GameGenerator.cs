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

public class MathHelper
{
	// taken from http://wiki.unity3d.com/index.php/3d_Math_functions
	//Two non-parallel lines which may or may not touch each other have a point on each line which are closest
	//to each other. This function finds those two points. If the lines are not parallel, the function 
	//outputs true, otherwise false.
	public static bool ClosestPointsOnTwoLines(out Vector3 closestPointLine1, out Vector3 closestPointLine2, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
		
		closestPointLine1 = Vector3.zero;
		closestPointLine2 = Vector3.zero;
		
		float a = Vector3.Dot(lineVec1, lineVec1);
		float b = Vector3.Dot(lineVec1, lineVec2);
		float e = Vector3.Dot(lineVec2, lineVec2);
		
		float d = a * e - b * b;
		
		//lines are not parallel
		if(d != 0.0f)
		{	
			Vector3 r = linePoint1 - linePoint2;
			float c = Vector3.Dot(lineVec1, r);
			float f = Vector3.Dot(lineVec2, r);
			
			float s = (b * f - c * e) / d;
			float t = (a * f - c * b) / d;
			
			closestPointLine1 = linePoint1 + lineVec1 * s;
			closestPointLine2 = linePoint2 + lineVec2 * t;
			
			return true;
		}
		else
		{
			return false;
		}
	}

	//Calculate the intersection point of two lines. Returns true if lines intersect, otherwise false.
	//Note that in 3d, two lines do not intersect most of the time. So if the two lines are not in the 
	//same plane, use ClosestPointsOnTwoLines() instead.
	public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1, Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){
		
		intersection = Vector3.zero;
		
		Vector3 lineVec3 = linePoint2 - linePoint1;
		Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
		Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);
		
		float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);
		
		//Lines are not coplanar. Take into account rounding errors.
		if((planarFactor >= 0.00001f) || (planarFactor <= -0.00001f)){
			
			return false;
		}
		
		//Note: sqrMagnitude does x*x+y*y+z*z on the input vector.
		float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
		
		if((s >= 0.0f) && (s <= 1.0f)){
			
			intersection = linePoint1 + (lineVec1 * s);
			return true;
		}
		
		else{
			return false;       
		}
	}
}


public delegate void WallObserver(Vector3 start, Vector3 end);

public interface IWalls
{
	void IterateWalls(WallObserver observer);
	float MinimumDistance(Vector3 point);
}

public delegate void PathCheckObserver(Vector3 a, Vector3 b);

public interface IPathChecks
{
	void IntersectEdges(Vector3 position, PathCheckObserver observer);
}

public class PathNode
{
	public float width = 0.0f;
	public Vector3 position;
	public Vector3 direction;
}

public class Path : IWalls
{
	static float max_width = 35.0f;
	static float min_width = 12.0f;
	static float max_width_variance = 2.0f;
	static float min_width_variance = 0.5f;
	
	static float max_turn = 30.0f;
	static float min_turn = 1.0f;
	
	static float max_edge_length = 10.0f;
	static float min_edge_length = 2.0f;

	private ArrayList m_points = new ArrayList();
	private IPathChecks m_path_checks;

	private Rect m_bounding_box;

	public void CalculateBoundingBox(Vector3 start, Vector3 end)
	{
		m_bounding_box.xMin = Mathf.Min (m_bounding_box.xMin, start.x, end.x);
		m_bounding_box.yMin = Mathf.Min (m_bounding_box.yMin, start.y, end.y);
		m_bounding_box.xMax = Mathf.Max (m_bounding_box.xMax, start.x, end.x);
		m_bounding_box.yMax = Mathf.Max (m_bounding_box.yMax, start.y, end.y);
	}
	
	private delegate void CheckWidthForAngle(Vector3 cross);

	public Path(IPathChecks path_checks, Vector3 position, Vector3 direction, float width)
	{
		m_path_checks = path_checks;

		for (int i = 0; i < 50; i++) {
			var node = new PathNode() { 
				width = width,
				position = position,
				direction = direction
			};

			Wall.CreateDebug(node.position, node.position + direction);
			Wall.CreateDebug(node.position, node.position + Left (node));
			Wall.CreateDebug(node.position, node.position + Right (node));

			m_points.Add (node);

			var next_distance = Random.Range (min_edge_length, max_edge_length);

			var turn = (Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(min_turn, max_turn);
			var rotation = Quaternion.AngleAxis(turn, Vector3.back);
			var new_unit_direction = (rotation * direction).normalized;
			var new_direction = new_unit_direction * next_distance;
			var new_position = position + direction;

			var next_max_width = max_width;
			
			var width_check = Vector3.Cross(new_unit_direction, Vector3.forward).normalized;

			CheckWidthForAngle check_width = (cross) =>
			{
				Vector3 result;

				MathHelper.LineLineIntersection(
					out result, 
                	node.position, Out(node, cross), 
                    new_position, next_max_width * Vector3.Cross(new_unit_direction, cross).normalized);

				next_max_width = Mathf.Min(next_max_width, (result - new_position).magnitude);
			};
			
			check_width(Vector3.forward);
			check_width(Vector3.back);

			var new_width = Mathf.Min (Mathf.Max(Random.Range(min_width_variance, max_width_variance) * width, min_width), next_max_width);

			position = new_position;
			direction = new_direction;
			width = new_width;
		}

		IterateWalls (new WallObserver (CalculateBoundingBox));
		
		//if (width_current > 8.0f && Random.value > 0.6f)
		//{
		//	Instantiate(Fish, (start + (end - start) / 2), Quaternion.AngleAxis(Random.value * 180, Vector3.forward));
		//}
	}
	
	private Vector3 Out(PathNode node, Vector3 cross)
	{
		return node.width * Vector3.Cross (node.direction, cross).normalized;
	}
			
	private Vector3 Left(PathNode node) { return Out(node, Vector3.back); }
	private Vector3 Right(PathNode node) { return Out(node, Vector3.forward); }

	public void EdgeToWall(PathNode previous, PathNode current, WallObserver observer)
	{	
		observer (previous.position + Right (previous), current.position + Right (current));
		observer (previous.position + Left (previous), current.position + Left (current));
	}

	public delegate void EdgeObserver(PathNode previous, PathNode current);

	public void IterateEdges(EdgeObserver observer)
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

			observer(previous, current);
		}
	}


	public void IterateWalls(WallObserver observer)
	{
		IterateEdges (new EdgeObserver ((a, b) => EdgeToWall (a, b, observer)));
	}

	public void CreateWalls()
	{
		IterateWalls (new WallObserver ((a, b) => { Wall.Create(a, b); }));
	}

	public float MinimumDistance(Vector3 position)
	{
		return new Vector2 (
			Mathf.Min (
				position.x - m_bounding_box.xMin,
				position.x - m_bounding_box.xMax),
			Mathf.Min (
				position.y - m_bounding_box.yMin,
				position.y - m_bounding_box.yMax)).magnitude;
	}
}

public class PathPoint
{
	public Vector3 position;
	public Vector3 direction;

	public PathPoint() {}
	public PathPoint(PathPoint copy)
	{
		position = copy.position;
		direction = copy.direction;
	}
}

public class Pathway
{
	public static float max_distance_difference = 4f;
	public static float min_distance_difference = 0f;

	public static float max_distance = 4f;
	public static float min_distance = 1f;


	public static float max_width = 40f;
	public static float min_width = 20f;
	
	private List<PathPoint> m_left = new List<PathPoint>();
	private List<PathPoint> m_right = new List<PathPoint>();
	private IPathChecks m_path_checks;
	
	public Pathway(IPathChecks path_checks, Vector3 position, Vector3 direction, float width)
	{
		m_path_checks = path_checks;
		
		PathPoint left = new PathPoint () { 
			position = position + Vector3.Cross(direction, Vector3.forward).normalized * width
		};

		PathPoint right = new PathPoint () { 
			position = position + Vector3.Cross(direction, Vector3.back).normalized * width
		};

		for(int i = 0; i < 20; i++)
		{
			var new_direction = Vector3.Cross(right.position - left.position, Vector3.forward).normalized;
			var distance = Random.Range(min_distance, max_distance);
			
			left.direction = (Random.Range(min_distance_difference, max_distance_difference) + distance) * new_direction;
			right.direction = (Random.Range(min_distance_difference, max_distance_difference) + distance) * new_direction;

			Wall.CreateDebug(left.position, left.position + left.direction);
			Wall.CreateDebug(right.position, right.position + right.direction);
			m_left.Add(new PathPoint(left));
			m_right.Add(new PathPoint(right));
			
			left.position = left.position + left.direction;
			right.position = right.position + right.direction;
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

public class Area : IWalls
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
	public List<AreaEntry> m_entries = new List<AreaEntry>();

	public float MinimumDistance(Vector3 position)
	{
		return Mathf.Max (Vector3.Distance (position, m_position) - max_radius, 0.0f);
	}

	private float AverageRemainingAngle(int current_point, float angle_used)
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

		for(int i = 0; i < m_entries.Count; i++)
		{
			Wall.CreateDebug(m_position, NodePosition(i));
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
		IterateWalls (new WallObserver ((a, b) => { Wall.Create(a, b); }));
	}

	public void IterateWalls (WallObserver observer)
	{
		for(int i = 0; i < m_entries.Count; i++)
		{
			if (!At (i).create)
				continue;

			observer(NodePosition(i), NodePosition(i + 1));
		}
	}
}

public class LevelManager : IPathChecks
{
	private List<Pathway> m_paths = new List<Pathway> ();
	private List<Area> m_areas = new List<Area> ();

	public void AddArea(Vector3 position)
	{
		m_areas.Add (new Area (position));
	}

	public void CreateWalls()
	{
		foreach (Area area in m_areas) {
			area.CreateWalls ();
		}
	}

	public static float grid_size = Area.max_radius * 5f;

	public LevelManager()
	{
		for(int i = 0; i < 10; i++)
		{
			Wall.CreateDebug(new Vector3(i * grid_size, 0, 0), new Vector3(i * grid_size, grid_size * 10, 0));
			Wall.CreateDebug(new Vector3(0, i * grid_size, 0), new Vector3(grid_size * 10, i * grid_size, 0));
		}

		for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
				AddArea(new Vector3(
					Random.Range(x * grid_size + Area.max_radius, (x + 1) * grid_size - Area.max_radius),
					Random.Range(y * grid_size + Area.max_radius, (y + 1) * grid_size - Area.max_radius),
					0
				));
			}
		}

		CreateWalls ();
		//var direction = wall.end - wall.start;
		//var p = new Pathway(this, wall.start + direction / 2, Vector3.Cross(direction.normalized, Vector3.back), direction.magnitude / 2);
		//p.CreateWalls ();

	}

	public void IntersectEdges (Vector3 position, PathCheckObserver observer)
	{
		foreach (var area_obj in m_areas)
			CheckObject (position, observer, area_obj as IWalls);
		foreach (var path_obj in m_paths)
			CheckObject (position, observer, path_obj as IWalls);
	}

	private void CheckObject(Vector3 position, PathCheckObserver observer, IWalls wall_object)
	{
		if (wall_object.MinimumDistance(position) == 0.0f)
		{
			wall_object.IterateWalls(new WallObserver((start, end) => { observer(start, end); }));
		}
	}
}



public class GameGenerator : MonoBehaviour {
	
	public GameObject WallTemplate;
	public Material DebugMaterialTemplate;

	// Use this for initialization
	void Start () {
		Wall.WallTemplate = WallTemplate;
		Wall.DebugMaterialTemplate = DebugMaterialTemplate;
		var manager = new LevelManager ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
