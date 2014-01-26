using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaEntry
{
	public float angle = 0.0f;
	public float radius = 0.0f;
	public bool create = true;
}

public class RemovedWall
{
	public Vector3 left;
	public Vector3 right;
}

public delegate void AreaWallDelegate(int wall_id, Vector3 start, Vector3 end);

public class Area : IArea
{
	public static float max_radius = 30.0f;
	public static float min_radius = 15.0f;
	public static float max_radius_variance = 1.5f;
	public static float min_radius_variance = 0.5f;
	
	public static float max_angle_variance = 1.5f;
	public static float min_angle_variance = 1f / 1.5f;
	
	public static int min_points = 8;
	public static int max_points = 24;
	
	public static float total_angle = 360f;

	public Vector3 position { get { return m_position; } } 

	public event PlayerEnteredDelegate PlayerEntered;
	public event PlayerExitedDelegate PlayerExisted;
	
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
	
	public RemovedWall UseWall(int wall)
	{
		(m_entries [wall] as AreaEntry).create = false;
		return new RemovedWall ()
		{
			left = NodePosition(wall),
			right = NodePosition(wall + 1)
		};
	}

	public Vector3 GetSpawnLocation ()
	{
		return RandomSpot ();
	}

	public Vector3 GetOriginLocation ()
	{
		return position + RandomSpot ().normalized;
	}

	public Vector3 RandomSpot()
	{
		Vector3 result = Vector3.zero;

		float angle = Random.Range (0f, total_angle);

		var direction = (Quaternion.AngleAxis (angle, Vector3.back) * Vector3.up).normalized;

		Wall.CreateDebug (position, position + direction * 5f);

		for(int i = 0; i < m_points; i++)
		{
			var this_angle = At (i).angle;
			var next_angle = (i + 1 == m_points) ? total_angle : At(i + 1).angle;
			if (this_angle <= angle && angle < next_angle)
			{
				var before = NodePosition(i);
				var after = NodePosition(i + 1);
				var distance_along = (angle - this_angle) / (next_angle - this_angle);
				var edge = before + distance_along * (after - before);
				var offset = edge - position;
				result = position + Random.value * (offset - new Vector3(0.5f, 0.5f, 0.0f));
				break;
			}
		}

		return result;
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