using UnityEngine;
using System.Collections;

public class GoodObject : AreaObject {

	void OnDestroy() {
		gameArea.RemoveGoodObject (this.gameObject);
	}
}
