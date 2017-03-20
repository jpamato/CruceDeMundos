using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelData : MonoBehaviour {

	public List<Level> levels;

	// Use this for initialization
	void Start () {	
	
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
		public List<DialogUnlock> dialogsUnlock;
		public GameObject levelObjects;
		public CameraController.Zoom zoomIn;
		public CameraController.Zoom zoomOut;
		public string layoutJson;
		public VisualCell labCell;
		public int resourceWin;
		public bool isTuto;
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
}
