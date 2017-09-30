using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour {

	public GameManager gameManager;

	public void AcceptEvent () {
		gameManager.ChangeLevel();
	}
}
