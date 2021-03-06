﻿using UnityEngine;
using System.Collections;

public class OnCharacterCollider : MonoBehaviour {

	public string charName;

	bool trigged;
	public bool blocking;
	public bool once;

	// Use this for initialization
	void Start () {
		Events.ResetCharacterCollider += ResetCharacterCollider;
		Events.CharacterBlocking += CharacterBlocking;
	}

	void OnDestroy(){
		Events.ResetCharacterCollider -= ResetCharacterCollider;
		Events.CharacterBlocking -= CharacterBlocking;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (!trigged&&other.tag == "Player") {
			Game.Instance.gameManager.state = GameManager.states.DIALOG;
			Game.Instance.dialogManager.LoadDialog (charName);
			if(!blocking)trigged = true;
			Events.GameDialog ();
		}
	}

	void OnTriggerStay2D(Collider2D other) {		
		if (other.attachedRigidbody && blocking)
			other.attachedRigidbody.AddForce(new Vector2((other.transform.position.x-transform.position.x)*1f,(other.transform.position.y-transform.position.y)*1f));
	}

	void ResetCharacterCollider(){
		if(!once)
		trigged = false;
	}

	void CharacterBlocking(string name, int b){
		if(charName==name)
			blocking = b>0?true:false;
	}
}
