using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void PlayerEnteredDelegate(Vector3 position);
public delegate void PlayerExitedDelegate(Vector3 position);

public interface IArea
{
	Vector3 GetSpawnLocation();
	Vector3 GetOriginLocation();
	event PlayerEnteredDelegate PlayerEntered;
	event PlayerExitedDelegate PlayerExisted;
}