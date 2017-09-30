using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public GameObject gameOverSound;
	[HideInInspector] public AudioSource gameOverSoundSource;

	// Use this for initialization
	void Start () {
		gameOverSoundSource = gameOverSound.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
