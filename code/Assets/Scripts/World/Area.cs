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

	public static int spawn_directions = 40;

	public Vector3 position { get { return m_position; } } 

	public GameArea gameArea { get { return m_gameArea; } }

	public event PlayerEnteredDelegate PlayerEntered;
	public event PlayerExitedDelegate PlayerExisted;

	public SortedDictionary<int, object> m_used_spawns = new SortedDictionary<int, object>();
	
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
				angle = angle_used
			};
			
			m_entries.Add(area_entry);
			
			old_radius = area_entry.radius;
			angle_used = area_entry.angle + Random.Range(min_angle_variance, max_angle_variance) * average_remaining;
		}
		
		for(int i = 0; i < m_entries.Count; i++)
		{
			Wall.CreateDebug(m_position, NodePosition(i));
		}
	}

	public void PlayerStartsHere()
	{
		m_player_inside = true;
		if (PlayerEntered != null)
			PlayerEntered (Player.player.gameObject.transform.position);
	}


	public void Load()
	{
		m_gameArea = new GameArea (this);
	}
	
	public RemovedWall UseWall(int wall)
	{
		(m_entries [wall] as AreaEntry).create = false;

		var result = new RemovedWall ()
		{
			left = NodePosition(wall),
			right = NodePosition(wall + 1)
		};

		var movement = Vector3.Cross (result.right - result.left, Vector3.back).normalized;
		
		var enter_detection = Wall.CreateDetector (result.left, result.right);
		var exit_detection = Wall.CreateDetector (result.left + movement, result.right + movement);
		enter_detection.PlayerHit += delegate(Vector3 position) {
			if (m_player_inside) return;
			m_player_inside = true;
			if (PlayerEntered != null)
				PlayerEntered(position);
		};
		exit_detection.PlayerHit += delegate(Vector3 position) {
			if (!m_player_inside) return;
			m_player_inside = false;
			if (PlayerExisted != null)
				PlayerExisted(position);
		};

		return result;
	}

	public Vector3 GetSpawnLocation ()
	{
		return RandomSpot ();
	}

	public Vector3 GetOriginLocation ()
	{
		return position + RandomSpot ().normalized + Vector3.forward * 10;
	}

	public Vector3 RandomSpot()
	{
		Vector3 result = Vector3.zero;

		int number = Random.Range (0, spawn_directions - m_used_spawns.Count);

		foreach (var used in m_used_spawns) {
			if (used.Key > number) break;
			number++;
		}

		float angle = number * total_angle / spawn_directions;

		m_used_spawns.Add(number, null);

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
				result = position + Random.Range(4f / offset.magnitude, 1f) * (offset - 4 * offset.normalized);
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

	private bool m_player_inside = false;
	private Vector3 m_position;
	private int m_points;
	private List<AreaEntry> m_entries = new List<AreaEntry>();
	private GameArea m_gameArea;
}