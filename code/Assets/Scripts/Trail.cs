using UnityEngine;
using System.Collections;

public class TrailEndPoint
{
	public TrailEndPoint(Vector3 left, Vector3 right)
	{
		position = left + (right - left) / 2;
		direction = Vector3.Cross(right - left, Vector3.forward).normalized;
		radius = Vector3.Distance (left, right) / 2f;
	}
	
	public Vector3 GetLeft()
	{
		return position + radius * Vector3.Cross (direction, Vector3.back).normalized;
	}
	
	public Vector3 GetRight()
	{
		return position + radius * Vector3.Cross (direction, Vector3.forward).normalized;
	}
	
	private Vector3 position { get; set; }
	private Vector3 direction { get; set; }
	private float radius { get; set; }
}

public class Trail 
{
	
	private TrailEndPoint m_start;
	private TrailEndPoint m_end;

	public Trail(Vector3 start_left, Vector3 start_right, Vector3 end_left, Vector3 end_right)
	{
		Wall.CreateDebug (start_left, end_left);
		Wall.CreateDebug (start_right, end_right);
		m_start = new TrailEndPoint (start_left, start_right);
		m_end = new TrailEndPoint (end_left, end_right);
	}

	public void MakeWall(TrailEndPoint start, TrailEndPoint end)
	{
		Wall.Create (start.GetLeft (), end.GetLeft ());
		Wall.Create (start.GetRight (), end.GetRight ());
	}

	public void CreateWalls()
	{
		MakeWall (m_start, m_end);
	}
}
