using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

	public int ingredientType;

	void OnCollisionEnter2D(Collision2D other)
	{
		Chef c = other.gameObject.GetComponent<Chef> ();

		if (c != null) {
			c.AddIngredient(ingredientType);
			Debug.Log ("Ingredient found!");
			Destroy (this.gameObject);
		}
	}
}
