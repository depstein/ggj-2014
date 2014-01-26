using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	
	public bool connected = false;
	
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
	
	public MissingWall UseWall(int wall)
	{
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
		IterateWalls (new WallObserver ((id, start, end) => { Wall.Create(start, end); }));
	}
	
	public void IterateWalls (WallObserver observer)
	{
		for(int i = 0; i < m_entries.Count; i++)
		{
			if (!At (i).create)
				continue;
			
			observer(i, NodePosition(i), NodePosition(i + 1));
		}
	}
}