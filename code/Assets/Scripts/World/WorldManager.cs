using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldManager
{
	public static WorldManager worldManager;
	public static GameObject GrassTemplate;
	public static int grid_count = 4;
	public static float grid_size = Area.max_radius * 3f;

	public void Load()
	{
		var dim = grid_size * grid_count / 2;
		var tiling_count = Mathf.Floor ((grid_size * grid_count) / 4.0f);

		var grass = GameObject.Instantiate (GrassTemplate, new Vector3 (dim, dim, 500), Quaternion.identity) as GameObject;
		grass.transform.localScale = new Vector3 (2 * dim, 2 * dim, 1);
		grass.GetComponent<MeshRenderer>().material.SetTextureScale("_MainTex", new Vector2(tiling_count, tiling_count));

		for(int i = 0; i < grid_count; i++)
		{
			Wall.CreateDebug(new Vector3(i * grid_size, 0, 0), new Vector3(i * grid_size, grid_size * grid_count, 0));
			Wall.CreateDebug(new Vector3(0, i * grid_size, 0), new Vector3(grid_size * grid_count, i * grid_size, 0));
		}
		
		for (int x = 0; x < grid_count; x++) 
		{
			for (int y = 0; y < grid_count; y++) 
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
			
			int wall_a_id = 0;
			IntersectLineWithArea(out wall_a_id, e.b.position, e.a.position - e.b.position, e.a);
			
			int wall_b_id = 0;
			IntersectLineWithArea(out wall_b_id, e.a.position, e.b.position - e.a.position, e.b);
			
			var wall_a = e.a.UseWall(wall_a_id);
			var wall_b = e.b.UseWall(wall_b_id);
			
			m_trails.Add(new Trail(wall_a.left, wall_a.right, wall_b.right, wall_b.left));
			
			Wall.CreateDebug(e.a.position, e.b.position);
		}
		
		CreateWalls ();
		
		foreach (var area in m_areas)
		{
			area.Load();
		}
	}

	public void AddArea(Vector3 position)
	{
		m_areas.Add (new Area (position));
	}
	
	public void CreateWalls()
	{
		foreach (Area area in m_areas) {
			area.CreateWalls ();
		}
		foreach (Trail trail in m_trails) {
			trail.CreateWalls ();
		}
	}

	public void OnThemeChanged()
	{

	}

	public Area PickRandomArea()
	{
		return m_areas [Random.Range (0, m_areas.Count)];
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

	private List<Trail> m_trails = new List<Trail> ();
	private List<Area> m_areas = new List<Area> ();
}
