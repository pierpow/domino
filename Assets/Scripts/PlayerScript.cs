using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public string playerName = "robert";
	public Color color = new Color(1, 1, 1, 1);

	public void ChangeColor(Color newColor) {
		color = newColor;
		SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
		sr.color = color;
	}
}
