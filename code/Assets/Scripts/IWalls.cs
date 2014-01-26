using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void WallObserver(Vector3 start, Vector3 end);

public interface IWalls
{
	void IterateWalls(WallObserver observer);
	float MinimumDistance(Vector3 point);
}