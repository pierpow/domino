using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {
	const string INITIAL_PLAYER_INTRODUCTION = "Vous avez {0} ans. Vous ne supportez plus la souffrance dans votre pays.";
	const string NEW_PLAYER_INTRODUCTION = "Un s'est fait taire mais le mouvement continue. De nouvelles voix s'èlèvent. Vous en incarnez une nouvelle. Vous avez {0} ans.";

	public int age = 20;
	public Color color = new Color(1, 1, 1, 1);

	public Text characterIntroductionText;
	public GameObject hat;

	public void InitialPlayer() {
		NewPlayer();
		characterIntroductionText.text = string.Format(INITIAL_PLAYER_INTRODUCTION, age);
	}

	public void NewPlayer() {
		GenerateColor();
		GenerateAge();
		characterIntroductionText.text = string.Format(NEW_PLAYER_INTRODUCTION, age);
	}

	void GenerateAge() {
		age = 22;
	}

	void GenerateColor() {
		float red = Random.Range(0.0f, 1.0f);
		float green = Random.Range(0.0f, 1.0f);
		float blue = Random.Range(0.0f, 1.0f);

		color = new Color(red, green, blue, 1);

		SpriteRenderer sr = hat.GetComponent<SpriteRenderer>();
		sr.color = color;
	}
}
