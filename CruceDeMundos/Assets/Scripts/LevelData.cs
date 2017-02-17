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
		public string objective1;
		public string objective2;
		public int fireNumber;
		public int portalNumber;
		public int pollutionNumber;
		public List<ObstacleObjective> obstacleObjectives;
		public List<DialogUnlock> dialogsUnlock;
		public GameObject levelObjects;
	}

	[Serializable]
	public class ObstacleObjective
	{
		public string tag;
		public int number;
	}

	[Serializable]
	public class DialogUnlock
	{		
		public string characterName;
		public int goTo;
		public List<ObstacleObjective> obstacleObjectives;
	}
}
