using UnityEngine;
using System.Collections;

public class Sheep : BadObject {

	public bool isInRegion = false;
	
	// Use this for initialization
	void Start ()
	{
	}

	public override bool ShouldCheckForMove()
	{
		return !isInRegion;
	}
}
