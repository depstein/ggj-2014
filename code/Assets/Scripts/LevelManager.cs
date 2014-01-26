using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public void MakePathway(Vector3 start_left, Vector3 start_right, Vector3 end_left, Vector3 end_right)
	{

	}
	
	public static float grid_size = Area.max_radius * 5f;

	void IntersectLineWithArea(out int wall_id, Vector3 line_position, Vector3 line_vector, Area target)
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
		
		for (int x = 0; x < 10; x++) {
			for (int y = 0; y < 10; y++) {
				AddArea(new Vector3(
					Random.Range(x * grid_size + Area.max_radius, (x + 1) * grid_size - Area.max_radius),
					Random.Range(y * grid_size + Area.max_radius, (y + 1) * grid_size - Area.max_radius),
					0));
			}
		}
		
		List<Edge> mst = ConnectAreas ();
		foreach (Edge e in mst) {
			var connection_position = e.a.m_position;
			var connection_vector = e.b.m_position - e.a.m_position;

			int wall_a = 0;
			IntersectLineWithArea(out wall_a, e.b.m_position, e.a.m_position - e.b.m_position, e.a);

			int wall_b = 0;
			IntersectLineWithArea(out wall_b, e.a.m_position, e.b.m_position - e.a.m_position, e.b);
			
			e.a.UseWall(wall_a);
			e.b.UseWall(wall_b);

			Wall.CreateDebug(e.a.m_position, e.b.m_position);
		}

		CreateWalls ();
		
	}
	
	public List<Edge> ConnectAreas() {
		List<Edge> edges = new List<Edge> ();
		for (int i=0; i<m_areas.Count; i++) {
			for(int j=i+1; j<m_areas.Count;j++) {
				edges.Add (new Edge(m_areas[i], m_areas[j]));
			}
		}
		edges.Sort ((x, y) => x.distance.CompareTo (y.distance));
		
		List<Edge> mst = new List<Edge> ();
		edges [0].a.connected = true;
		for(int i=0;i<edges.Count; i++) {
			if(edges[i].a.connected != edges[i].b.connected) {
				mst.Add (edges[i]);
				edges[i].a.connected = true;
				edges[i].b.connected = true;
				i = 0; //TODO: if we increase V substantially, this will grind to a halt.
			}
		}
		
		return mst;
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
			wall_object.IterateWalls(new WallObserver((id, start, end) => { observer(start, end); }));
		}
	}
}
