﻿using UnityEngine;
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
		AUTOEVAL,
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

		Game.Instance.levelMetrics.levelBeginTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.map1BeginTime = Game.Instance.levelMetrics.levelBeginTime;
		Game.Instance.levelMetrics.rtBegin = Data.Instance.playerData.resources;

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
		Game.Instance.levelMetrics.map1EndTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.objectivesBeginTime = Game.Instance.levelMetrics.map1EndTime;
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		state = states.MISION;
		Events.GameMision ();
	}

	public void Tools(){
		Game.Instance.levelMetrics.objectivesEndTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.toolsBeginTime = Game.Instance.levelMetrics.objectivesEndTime;
		state = states.TOOLS;
		Events.GameTools ();
	}

	public void AutoEval(){
		Game.Instance.levelMetrics.toolsEndTime = Time.realtimeSinceStartup;
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Game.Instance.toolsManager.SelectedToolsData ();
		Game.Instance.levelMetrics.rtPostTools = Data.Instance.playerData.resources;
		state = states.AUTOEVAL;
		Game.Instance.dialogManager.LoadDialog ("Dra Grimberg");
		Events.GameAutoeval ();
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
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Events.StartGame ();
		gameStarted=true;
	}

	public void Phone(){
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
