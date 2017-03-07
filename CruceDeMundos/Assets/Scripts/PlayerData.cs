using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerData : MonoBehaviour {

	public int level;
	public int resources;
	public int toolsNumber;
	public List<Level> summary;

	public enum ToolName{
		Matafuegos,
		Restaurador
	}

	public GameObject[] tools;

	[Serializable]
	public class Level
	{
		public int levelNumber;
		public bool[] objectivesDone = new bool[] {false,false,false};
	}

	public void SetSummary (){
		int lNumber = Game.Instance.levelManager.leveldata.levelNumber;
		bool[] oDone = Game.Instance.levelManager.objectivesDone;
		Level level = summary.Find (x => x.levelNumber == lNumber);

		if (level == null) {
			level = new Level ();
			level.levelNumber = lNumber;
			for (int i = 0; i < level.objectivesDone.Length; i++)
				if (!level.objectivesDone [i])
					level.objectivesDone [i] = oDone [i];			
			summary.Add (level);
		} else {
			for (int i = 0; i < level.objectivesDone.Length; i++)
				if (!level.objectivesDone [i])
					level.objectivesDone [i] = oDone [i];
		}

	}

}
