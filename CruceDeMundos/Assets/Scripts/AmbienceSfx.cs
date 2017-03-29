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
		if (Data.Instance.playerData.level < 4 && Data.Instance.playerData.level < 10)
			source.clip = ambience1_3;
		else
			source.clip = ambience2;

		source.Play ();
			
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
