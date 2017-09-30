using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	GameManager gameManager;

	void Start() {
		gameManager = FindObjectOfType<GameManager>();
	}

	public void AcceptEvent () {
		gameManager.ChangeLevelAfterAccept();
	}
}
