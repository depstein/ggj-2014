﻿using UnityEngine;
using System.Collections;

public class Shout: MonoBehaviour {
	
	private float timeLeft = 1f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if (timeLeft <= 0) {
			Destroy(this.gameObject);
				}
	}
}
