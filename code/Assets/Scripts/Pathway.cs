using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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