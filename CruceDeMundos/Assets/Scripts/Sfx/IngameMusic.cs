﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMusic : MonoBehaviour {

	public AudioClip musicLevel;
	public AudioClip musicLevelTime;
	public AudioClip musicLevelWin;
	public AudioClip musicLevelTimeOut;

	public float vol;
	public float thresh;

	AudioSource source;
	public bool init;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		Invoke ("Init", 1);
	}

	void Init () {		
		if (Game.Instance.levelManager.leveldata.isVsTime)
			source.clip = musicLevelTime;
		else
			source.clip = musicLevel;

		source.Play ();
		init = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (!source.isPlaying && init) {
			source.Play ();
			if (Random.value < thresh)
				source.volume = vol;
			else
				source.volume = 0f;
		}
	}

	public void MusicWin(){
		init = false;
		source.clip = musicLevelWin;
		source.volume = vol;
		source.Play ();
	}

	public void MusicTimeOut(){
		init = false;
		source.clip = musicLevelTimeOut;
		source.volume = vol;
		source.Play ();
	}
}
