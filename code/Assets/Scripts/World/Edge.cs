using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Edge 
{
	public Area a { get { return m_a; } }
	public Area b { get { return m_b; } }
	
	public Edge(Area a, Area b)
	{
		m_a = a;
		m_b = b;
		m_distance = Vector3.Distance (a.position, b.position);
	}

	public int CompareTo(Edge edge)
	{
		return m_distance.CompareTo (edge.m_distance);
	}

	private Area m_a;
	private Area m_b;
	private float m_distance;
}
