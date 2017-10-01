using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public string playerName = "robert";
	public Color color = new Color(1, 1, 1, 1);

	public void ChangeColor() {
		float red = Random.Range(0.0f, 1.0f);
		float green = Random.Range(0.0f, 1.0f);
		float blue = Random.Range(0.0f, 1.0f);

		color = new Color(red, green, blue, 1);

		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.color = color;
	}
}
