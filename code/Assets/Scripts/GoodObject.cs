using UnityEngine;
using System.Collections;

public class GoodObject : MonoBehaviour {
	// Use this for initialization
	protected virtual void Start () {
		GameArea.gameArea.AddGoodObject (this.gameObject);
	}

	void OnDestroy() {
		GameArea.gameArea.RemoveGoodObject (this.gameObject);
	}
}
