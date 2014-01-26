using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	public void CalculateBoundingBox(int wall_id, Vector3 start, Vector3 end)
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
	
	public void EdgeToWall(int id, PathNode previous, PathNode current, WallObserver observer)
	{	
		observer (id, previous.position + Right (previous), current.position + Right (current));
		observer (id, previous.position + Left (previous), current.position + Left (current));
	}
	
	public delegate void EdgeObserver(int id, PathNode previous, PathNode current);
	
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
			
			observer(-1, previous, current);
		}
	}
	
	
	public void IterateWalls(WallObserver observer)
	{
		IterateEdges (new EdgeObserver ((id, a, b) => EdgeToWall (id, a, b, observer)));
	}
	
	public void CreateWalls()
	{
		IterateWalls (new WallObserver ((id, a, b) => { Wall.Create(a, b); }));
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