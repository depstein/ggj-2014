using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Edge {
	public Area a;
	public Area b;
	public float distance;
	
	public Edge(Area a, Area b) {
		this.a = a;
		this.b = b;
		this.distance = Vector3.Distance (a.m_position, b.m_position);
	}
}
