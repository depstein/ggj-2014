using UnityEngine;
using System.Collections;

public class GoodObject : AreaObject {

	void OnDestroy() {
		Debug.Log (this.gameObject.name + " is dead");
		gameArea.RemoveGoodObject (this.gameObject);
	}
}
