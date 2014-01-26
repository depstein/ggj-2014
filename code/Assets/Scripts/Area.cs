using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaEntry
{
	public float angle = 0.0f;
	public float radius = 0.0f;
	public bool create = true;
}

public delegate void AreaWallDelegate(int wall_id, Vector3 start, Vector3 end);

public class Area
{
	public static float max_radius = 50.0f;
	public static float min_radius = 20.0f;
	public static float max_radius_variance = 1.5f;
	public static float min_radius_variance = 0.5f;
	
	public static float max_angle_variance = 1.5f;
	public static float min_angle_variance = 1f / 1.5f;
	
	public static int min_points = 8;
	public static int max_points = 24;
	
	public static float total_angle = 360f;

	public Vector3 position { get { return m_position; } } 
	
	public void UseWall(int wall)
	{
		(m_entries [wall] as AreaEntry).create = false;
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
			
			m_entries.Add(area_entry);
			
			old_radius = area_entry.radius;
			angle_used = area_entry.angle;
		}
		
		for(int i = 0; i < m_entries.Count; i++)
		{
			Wall.CreateDebug(m_position, NodePosition(i));
		}
	}
	
	public void CreateWalls()
	{
		IterateWalls (new AreaWallDelegate ((id, start, end) => { Wall.Create(start, end); }));
	}
	
	public void IterateWalls (AreaWallDelegate observer)
	{
		for(int i = 0; i < m_entries.Count; i++)
		{
			if (!At (i).create)
				continue;
			
			observer(i, NodePosition(i), NodePosition(i + 1));
		}
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

		return m_position + direction;
	}
	
	private float AverageRemainingAngle(int current_point, float angle_used)
	{
		var remaining_angle = total_angle - angle_used;
		var remaining_points = m_points - current_point;
		
		return remaining_angle / remaining_points;
	}
	
	private Vector3 m_position;
	private int m_points;
	private List<AreaEntry> m_entries = new List<AreaEntry>();
}