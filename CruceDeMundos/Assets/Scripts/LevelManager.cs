using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public GameObject container;
	public CameraController camControl;

	public LevelData.Level leveldata;
	public bool[] objectivesDone = new bool[] {false,false,false};

	public List<LevelData.TimeObjective> timeObjectives;
	public List<LevelData.ObstacleObjective> obstacleObjectives;
	public List<LevelData.DialogUnlock> dialogsUnlock;

	public int tools;

	// Use this for initialization
	void Awake () {
		leveldata = Data.Instance.levelData.levels.Find (x => x.levelNumber == Data.Instance.playerData.level);

		timeObjectives = new List<LevelData.TimeObjective> ();
		foreach (LevelData.TimeObjective tObjective in leveldata.timeObjectives) {
			LevelData.TimeObjective to = new LevelData.TimeObjective ();
			to.timeOut = tObjective.timeOut;
			to.objectiveIndex = tObjective.objectiveIndex;
			timeObjectives.Add (to);
		}

		obstacleObjectives = new List<LevelData.ObstacleObjective> ();
		foreach (LevelData.ObstacleObjective oObjective in leveldata.obstacleObjectives) {
			LevelData.ObstacleObjective oo = new LevelData.ObstacleObjective ();
			oo.number = oObjective.number;
			oo.tag = oObjective.tag;
			oo.objectiveIndex = oObjective.objectiveIndex;
			obstacleObjectives.Add (oo);
		}

		dialogsUnlock = new List<LevelData.DialogUnlock> ();
		foreach (LevelData.DialogUnlock dUnlock in leveldata.dialogsUnlock) {
			LevelData.DialogUnlock dU = new LevelData.DialogUnlock ();
			dU.characterName = dUnlock.characterName;
			dU.goTo = dUnlock.goTo;
			dU.obstacleObjectives = new List<LevelData.ObstacleObjective> ();
			foreach (LevelData.ObstacleObjective oObjective in dUnlock.obstacleObjectives) {
				LevelData.ObstacleObjective oo = new LevelData.ObstacleObjective ();
				oo.number = oObjective.number;
				oo.tag = oObjective.tag;
				dU.obstacleObjectives.Add (oo);
			}
			dialogsUnlock.Add (dU);
		}

		tools = Data.Instance.playerData.toolsNumber;

		camControl.zoomIn = leveldata.zoomIn;
		camControl.zoomOut = leveldata.zoomOut;
		Instantiate (leveldata.levelObjects, container.transform);

		MazeGenerator mz = container.GetComponentInChildren<MazeGenerator> ();
		mz.visualCellPrefab = leveldata.labCell;
		mz.jsonName = leveldata.layoutJson;
	}

	void Start () {		
		Events.OnObstacleDestroy += OnObstacleDestroy;
	}

	void OnDestroy(){		
		Events.OnObstacleDestroy -= OnObstacleDestroy;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnObstacleDestroy(string tag){				
		for(int i=0;i<obstacleObjectives.Count;i++){
			LevelData.ObstacleObjective oObjective = obstacleObjectives[i];
			if (oObjective.tag == tag) {				
				oObjective.number--;
				if (oObjective.number <= 0) {
					objectivesDone [oObjective.objectiveIndex] = true;
					obstacleObjectives.Remove (oObjective);									
				}
			}
		}

		for(int j=0;j<dialogsUnlock.Count;j++)
			for(int i=0;i<dialogsUnlock[j].obstacleObjectives.Count;i++){
				LevelData.ObstacleObjective oObjective = dialogsUnlock[j].obstacleObjectives[i];
				if (oObjective.tag == tag) {				
					oObjective.number--;
					if (oObjective.number <= 0) {
						dialogsUnlock[j].obstacleObjectives.Remove (oObjective);
						if (dialogsUnlock[j].obstacleObjectives.Count == 0)							
							Game.Instance.dialogManager.UnlockDialog (dialogsUnlock[j].characterName, leveldata.levelNumber, dialogsUnlock[j].goTo);
					}
				}
			}
	}

	public void CheckTimeObjective(float time){
		for(int i=0;i<timeObjectives.Count;i++){
			LevelData.TimeObjective tObjective = timeObjectives[i];
			if (time < tObjective.timeOut) {				
				objectivesDone [tObjective.objectiveIndex] = true;
			}
		}
	}

	public void ToolLose(){
		tools--;
		if (tools <= 0)
			Events.OnToolsLose ();
	}
}
