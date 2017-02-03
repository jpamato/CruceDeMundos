using UnityEngine;
using System.Collections;

public class OnCharacterCollider : MonoBehaviour {

	public string charName;

	bool trigged;

	// Use this for initialization
	void Start () {
		Events.ResetCharacterCollider += ResetCharacterCollider;
	}

	void OnDestroy(){
		Events.ResetCharacterCollider -= ResetCharacterCollider;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (!trigged&&other.tag == "Player") {
			Game.Instance.gameManager.state = GameManager.states.DIALOG;
			Game.Instance.dialogManager.LoadDialog (charName);
			trigged = true;
			Events.GameDialog ();
		}
	}

	void ResetCharacterCollider(){
		trigged = false;
	}
}
