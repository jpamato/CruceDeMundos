using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfieUI : MonoBehaviour {

	public ToggleButton[] emojis;
	public string[] animNames;

	Animator animator;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetEmoji(int index){
		for (int i = 0; i < emojis.Length; i++) {			
			emojis[i].SetButtonOn (i == index);
		}
		Debug.Log (animNames [index]);
		animator.Play (""+animNames[index]);
		//animator.Play ("contento");
	}
}
