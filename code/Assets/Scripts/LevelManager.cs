using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager
{
	public static float grid_size = Area.max_radius * 5f;
	
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

	public void MakePathway(Vector3 start_left, Vector3 start_right, Vector3 end_left, Vector3 end_right)
	{

	}

	private void IntersectLineWithArea(out int wall_id, Vector3 line_position, Vector3 line_vector, Area target)
	{
		float closest = -1f;
		int result = -1;

		target.IterateWalls (
			(id, start, end) => 
			{
				var wall_position = start;
				var wall_vector = end - start;

				Vector3 intersection;

				if (MathHelper.LineLineIntersection (
					out intersection,
					wall_position,
					wall_vector,
					line_position,
					line_vector)) 
				{
					var distance = Vector3.Distance(line_position, intersection);
					if (closest < 0 || distance < closest)
					{
						closest = distance;
						result = id;
					}
				}
			});

		wall_id = result;
	}

	public LevelManager()
	{
		for(int i = 0; i < 10; i++)
		{
			Wall.CreateDebug(new Vector3(i * grid_size, 0, 0), new Vector3(i * grid_size, grid_size * 10, 0));
			Wall.CreateDebug(new Vector3(0, i * grid_size, 0), new Vector3(grid_size * 10, i * grid_size, 0));
		}
		
		for (int x = 0; x < 10; x++) 
		{
			for (int y = 0; y < 10; y++) 
			{
				AddArea(new Vector3(
					Random.Range(x * grid_size + Area.max_radius, (x + 1) * grid_size - Area.max_radius),
					Random.Range(y * grid_size + Area.max_radius, (y + 1) * grid_size - Area.max_radius),
					0));
			}
		}
		
		List<Edge> mst = MinimumSpanningTree.ConnectAreas (m_areas);
		foreach (Edge e in mst)
		{
			var connection_position = e.a.position;
			var connection_vector = e.b.position - e.a.position;

			int wall_a = 0;
			IntersectLineWithArea(out wall_a, e.b.position, e.a.position - e.b.position, e.a);

			int wall_b = 0;
			IntersectLineWithArea(out wall_b, e.a.position, e.b.position - e.a.position, e.b);
			
			e.a.UseWall(wall_a);
			e.b.UseWall(wall_b);

			Wall.CreateDebug(e.a.position, e.b.position);
		}

		CreateWalls ();
		
	}

	private List<Area> m_areas = new List<Area> ();
}
