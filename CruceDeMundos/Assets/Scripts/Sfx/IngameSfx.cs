using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSfx : MonoBehaviour {

	public AudioClip dialog;
	public AudioClip timeAlarm;

	private AudioSource source;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
	}

	void OnDestroy(){
	}

	public void PlaySfx(AudioClip ac){
		source.PlayOneShot (ac);
	}
}
