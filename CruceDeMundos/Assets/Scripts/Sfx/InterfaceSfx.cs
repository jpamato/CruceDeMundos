using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSfx : MonoBehaviour {

	public AudioClip click1;
	public AudioClip click2;
	public AudioClip over;

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