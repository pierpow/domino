using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {

	public GameObject musicLevel1;
	[HideInInspector] public AudioSource musicLevel1Source;

	public GameObject musicLevel5;
	[HideInInspector] public AudioSource musicLevel5Source;

	private int currentlyPlaying = 0;

	void Awake () {
		musicLevel1Source = musicLevel1.GetComponent<AudioSource>();
		musicLevel5Source = musicLevel5.GetComponent<AudioSource>();
	}

	public void PlayMusicForLevel(int level) {
		if (currentlyPlaying == 0) {
			currentlyPlaying = 1;
			musicLevel1Source.Play();
			return;
		}
		// Nastiest piece of shit
		if (level < 50) {
			if (currentlyPlaying != 1) {
				currentlyPlaying = 1;
				musicLevel1Source.Play();
				musicLevel5Source.Stop();
			}
		} else {
			if (currentlyPlaying != 5) {
				currentlyPlaying = 5;
				musicLevel1Source.Stop();
				musicLevel5Source.Play();
			}
		}
	}
}
