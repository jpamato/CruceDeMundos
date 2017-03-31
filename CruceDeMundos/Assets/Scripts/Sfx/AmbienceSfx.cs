using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSfx : MonoBehaviour {

	public AudioClip ambience1_3;
	public AudioClip ambience2;

	AudioSource source;

	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
		Invoke ("Init", 1);			
	}

	void Init(){		
		if (Game.Instance.levelManager.leveldata.levelNumber < 5 || Game.Instance.levelManager.leveldata.levelNumber > 10)
			source.clip = ambience1_3;
		else
			source.clip = ambience2;

		source.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
