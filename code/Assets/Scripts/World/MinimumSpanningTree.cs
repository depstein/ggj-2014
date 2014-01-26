using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimumSpanningTree {

	public static List<Edge> ConnectAreas(List<Area> areas) 
	{
		List<Edge> edges = new List<Edge> ();
		for (int i = 0; i < areas.Count; i++) 
		{
			for(int j = i + 1; j < areas.Count; j++) 
			{
				edges.Add (new Edge(areas[i], areas[j]));
			}
		}

		edges.Sort ((x, y) => x.CompareTo(y));
		
		HashSet<Area> connected = new HashSet<Area>();
		
		List<Edge> mst = new List<Edge> ();
		connected.Add(edges [0].a);
		for(int i = 0;i < edges.Count; i++) 
		{
			if(connected.Contains(edges[i].a) != connected.Contains(edges[i].b)) 
			{
				mst.Add (edges[i]);
				connected.Add(edges [i].a);
				connected.Add(edges [i].b);

				//	TODO: if we increase V substantially, this will grind to a halt.
				//	solution: erase edges from the list that do not meet the condition
				// 		so that they are no longer considered
				i = 0; 
			}
		}
		
		return mst;
	}
}
