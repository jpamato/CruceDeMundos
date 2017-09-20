using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {

	public AudioClip introMusic;
	public float introVol;
	public AudioClip ingameMusic;
	public float ingameVol;

	private AudioSource source;
	bool ingame;

	// Use this for initialization
	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
		MusicChange (SceneManager.GetActiveScene().name);
	}

	public void MusicChange(string scene){
		if (scene.Equals ("Game")) {
			ingame = true;
			source.clip = ingameMusic;
			source.volume = ingameVol;
			source.Play ();
		} else if (ingame) {
			ingame = false;
			source.clip = introMusic;
			source.volume = introVol;
			source.Play ();
		} else if (scene.Equals ("Fin")) {
			ingame = true;
			source.Stop ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
