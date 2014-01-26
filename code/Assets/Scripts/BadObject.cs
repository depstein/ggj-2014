using UnityEngine;
using System.Collections;

public class BadObject : AreaObject {

	void OnDestroy() {
		gameArea.RemoveBadObject (this.gameObject);
	}
}
