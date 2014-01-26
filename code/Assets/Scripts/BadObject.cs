using UnityEngine;
using System.Collections;

public class BadObject : MonoBehaviour {
	// Use this for initialization
	protected virtual void Start () {
		GameArea.gameArea.AddBadObject (this.gameObject);
	}

	void OnDestroy() {
		GameArea.gameArea.RemoveBadObject (this.gameObject);
	}
}
