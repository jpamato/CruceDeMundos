using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelData : MonoBehaviour {

	public bool saveMissionsToDatabase;

	public List<Level> levels;

	// Use this for initialization
	void Start () {
		if (saveMissionsToDatabase) {
			Debug.Log ("aca");
			foreach (Level l in levels) {
				string m = ""; 
				foreach (string o in l.objectives) {
					m += o + ";";
				}
				//StartCoroutine(Data.Instance.dataController.AddMission (l.levelNumber, m));
				StartCoroutine(Data.Instance.dataController.AddLevelData (l.levelNumber, m,l.portalNumber,l.fireNumber,l.pollutionNumber));
			}
		}
	}
	
	[Serializable]
	public class Level
	{
		public int levelNumber;
		public string title;
		public List<string> objectives;
		public bool isVsTime;
		public float timeOut;
		public int fireNumber;
		public int portalNumber;
		public int pollutionNumber;
		public List<TimeObjective> timeObjectives;
		public List<ObstacleObjective> obstacleObjectives;
		public List<ChargeObjective> chargeObjective;
		public List<DialogObjective> dialogObjective;
		public List<DialogUnlock> dialogsUnlock;
		public List<CharacterMove> characterMove;
		public GameObject levelObjects;
		public CameraController.Zoom zoomIn;
		public CameraController.Zoom zoomOut;
		public string layoutJson;
		public VisualCell labCell;
		public int resourceWin;
		public bool isTuto;
		public bool isImposible;
	}

	[Serializable]
	public class ObstacleObjective
	{
		public ShootObstacle.obstacleType tag;
		public int number;
		public int objectiveIndex;
	}

	[Serializable]
	public class ChargeObjective
	{
		public PlayerData.ToolName toolName;
		public int val;
		public int objectiveIndex;
	}

	[Serializable]
	public class DialogObjective
	{
		public string characterName;
		public int objectiveIndex;
	}

	[Serializable]
	public class TimeObjective
	{		
		public float timeOut;
		public int objectiveIndex;
	}

	[Serializable]
	public class DialogUnlock
	{		
		public string characterName;
		public int goTo;
		public List<ObstacleObjective> obstacleObjectives;
	}

	[Serializable]
	public class CharacterMove
	{		
		public string characterName;
		public Vector2[] positions;
	}
}
