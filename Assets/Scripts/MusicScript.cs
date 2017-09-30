using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour {

	public GameObject musicLevel1;
	[HideInInspector] public AudioSource musicLevel1Source;

	public GameObject musicLevel2;
	[HideInInspector] public AudioSource musicLevel2Source;

	public GameObject musicLevel3;
	[HideInInspector] public AudioSource musicLevel3Source;

	public GameObject musicLevel4;
	[HideInInspector] public AudioSource musicLevel4Source;

	public GameObject musicLevel5;
	[HideInInspector] public AudioSource musicLevel5Source;

	private int currentlyPlaying = 0;

	void Awake () {
		musicLevel1Source = musicLevel1.GetComponent<AudioSource>();
		musicLevel2Source = musicLevel2.GetComponent<AudioSource>();
		musicLevel3Source = musicLevel3.GetComponent<AudioSource>();
		musicLevel4Source = musicLevel4.GetComponent<AudioSource>();
		musicLevel5Source = musicLevel5.GetComponent<AudioSource>();
	}

	public void PlayMusicForLevel(int level) {
		if (currentlyPlaying == 0) {
			currentlyPlaying = 1;
			musicLevel1Source.Play();
			return;
		}
		// Nastiest piece of shit
		if (level <= 19) {
			if (currentlyPlaying != 1) {
				currentlyPlaying = 1;
				musicLevel1Source.Play();
				musicLevel2Source.Stop();
				musicLevel3Source.Stop();
				musicLevel4Source.Stop();
				musicLevel5Source.Stop();
			}
		} else if (19 < level && level < 40) {
			if (currentlyPlaying != 2) {
				currentlyPlaying = 2;
				musicLevel1Source.Stop();
				musicLevel2Source.Play();
				musicLevel3Source.Stop();
				musicLevel4Source.Stop();
				musicLevel5Source.Stop();
			}
		} else if (40 <= level && level < 60) {
			if (currentlyPlaying != 3) {
				currentlyPlaying = 3;
				musicLevel1Source.Stop();
				musicLevel2Source.Stop();
				musicLevel3Source.Play();
				musicLevel4Source.Stop();
				musicLevel5Source.Stop();
			}
		} else if (60 <= level && level < 80) {
			if (currentlyPlaying != 4) {
				currentlyPlaying = 4;
				musicLevel1Source.Stop();
				musicLevel2Source.Stop();
				musicLevel3Source.Stop();
				musicLevel4Source.Play();
				musicLevel5Source.Stop();
			}
		} else {
			if (currentlyPlaying != 5) {
				currentlyPlaying = 5;
				musicLevel1Source.Stop();
				musicLevel2Source.Stop();
				musicLevel3Source.Stop();
				musicLevel4Source.Stop();
				musicLevel5Source.Play();
			}
		}
	}
}
