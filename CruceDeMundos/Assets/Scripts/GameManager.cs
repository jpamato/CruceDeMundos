using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public int resources;
	public float time;
	public int tools;
	public int fires;

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
		Events.OnObstacleDestroy += OnObstacleDestroy;
		Events.OnRefreshResources (Data.Instance.playerData.resources);

		Invoke ("Intro", 1);
	}

	void OnDestroy(){
		Events.DialogDone -= DialogDone;
		Events.OnObstacleDestroy -= OnObstacleDestroy;
	}

	void Intro(){		
		Events.GameIntro ();
		Invoke ("Mision", 4);
	}

	void Mision(){
		state = states.MISION;
		Events.GameMision ();
	}

	public void Tools(){
		state = states.TOOLS;
		Events.GameTools ();
	}

	public void Hint(){
		state = states.HINT;
		Game.Instance.dialogManager.LoadDialog ("Dr Grimberg");
		Events.GameHint ();
	}

	void DialogDone(){		
		if (!gameStarted) {
			Events.StartGame ();
			gameStarted=true;
		} else {
			state = states.ACTIVE;
			Events.GameActive ();
		}
		Events.ResetCharacterCollider ();
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

	void OnObstacleDestroy(string tag){		
		if (tag == "FIRE") {
			fires--;
			if (fires <= 0) {
				Game.Instance.dialogManager.UnlockDialog ("Mork", 1, 4);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {		
	}
}
