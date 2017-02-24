using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {
	
	public float time;
	public int tools;

	public states state;
	public enum states
	{
		INTRO,
		MISION,
		TOOLS,
		HINT,
		ACTIVE,
		MAP,
		DIALOG,
		WIN,
		LOSE,
		ENDED
	}

	bool gameStarted = false;

	// Use this for initialization
	void Start () {
		state = states.INTRO;
		Events.DialogDone += DialogDone;
		Events.OnRefreshResources (Data.Instance.playerData.resources);

		Invoke ("Intro", 1);
	}

	void OnDestroy(){
		Events.DialogDone -= DialogDone;
	}

	void Intro(){		
		Events.GameIntro ();
		//Invoke ("Mision", 4);
	}

	public void Mision(){
		state = states.MISION;
		Events.GameMision ();
	}

	public void Tools(){
		state = states.TOOLS;
		Events.GameTools ();
	}

	public void Hint(){
		state = states.HINT;
		Game.Instance.dialogManager.LoadDialog ("Dra Grimberg");
		Events.GameHint ();
	}

	void DialogDone(){		
		if (!gameStarted) {
			Events.GameReady ();
		} else {
			state = states.ACTIVE;
			Events.GameActive ();
		}
		Events.ResetCharacterCollider ();
	}

	public void StartGame(){
		Events.StartGame ();
		gameStarted=true;
	}

	public void Map(){
		if (state == states.MAP) {
			state = states.ACTIVE;
			Events.GameActive ();
		} else {
			state = states.MAP;
			Events.GameMap();
		}
	}
	
	// Update is called once per frame
	void Update () {		
	}
}
