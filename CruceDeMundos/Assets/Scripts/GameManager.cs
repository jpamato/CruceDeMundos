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
		AUTOEVAL,
		ACTIVE,
		MAP,
		SELFIE,
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
		//Game.Instance.levelMetrics.objectivesEndTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.toolsBeginTime = Time.realtimeSinceStartup;
		state = states.TOOLS;
		Events.GameTools ();
	}

	public void AutoEval(){
		/*Game.Instance.toolsManager.SelectedToolsData ();
		Game.Instance.levelMetrics.toolsEndTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.rtPostTools = Data.Instance.playerData.resources;*/
		Game.Instance.levelMetrics.objectivesEndTime = Time.realtimeSinceStartup;

		if (Game.Instance.dialogManager.LoadInitialDialog ()){			
			Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
			state = states.AUTOEVAL;
			/*if(!Game.Instance.dialogManager.LoadInitialDialog ())
				Game.Instance.dialogManager.LoadDialog ("Dra Grimberg");*/
			Events.GameAutoeval ();
		}else{
			//Events.GameReady ();
			Tools();
		}
	}

	void DialogDone(){		
		if (!gameStarted) {
			//Events.GameReady ();
			Tools();
		} else {
			state = states.ACTIVE;
			Events.GameActive ();
		}

		Invoke ("ResetCharacterCollider", 2f);
	}

	public void GameReady(){
		Game.Instance.toolsManager.SelectedToolsData ();
		Game.Instance.levelMetrics.toolsEndTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.map2BeginTime = Time.realtimeSinceStartup;
		Game.Instance.levelMetrics.rtPostTools = Data.Instance.playerData.resources;
		Events.GameReady ();
	}

	void ResetCharacterCollider(){
		Events.ResetCharacterCollider ();
	}

	public void StartGame(){
		Game.Instance.levelMetrics.map1EndTime += Time.realtimeSinceStartup-Game.Instance.levelMetrics.map2BeginTime;
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Events.StartGame ();
		gameStarted=true;
	}

	public void Phone(){
		if (state == states.MAP) {
			Game.Instance.levelMetrics.map1EndTime += Time.realtimeSinceStartup-Game.Instance.levelMetrics.map2BeginTime;
			state = states.ACTIVE;
			Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.phoneClose);
			Events.GameActive ();
		} else {
			Game.Instance.levelMetrics.map2BeginTime = Time.realtimeSinceStartup;
			state = states.MAP;
			Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.phoneOpen);
			Events.GameMap();
		}
	}
	
	// Update is called once per frame
	void Update () {		
	}
}
