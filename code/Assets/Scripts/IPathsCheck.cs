using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public delegate void PathCheckObserver(Vector3 a, Vector3 b);

public interface IPathChecks
{
	void IntersectEdges(Vector3 position, PathCheckObserver observer);
}