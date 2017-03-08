using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using SimpleJSON;

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

	public string GetPlayerData(){
		string json = "playerData:{\n";
		json += "level:"+level+",\n";
		json += "resources:"+resources+",\n";
		json += "toolsNumber:"+toolsNumber+",\n";
		json += "summary:[\n";

		for(int i=0;i<summary.Count;i++){
			json += "{levelNumber:"+ summary[i].levelNumber + ",";
			json += "objectivesDone:[";
			for (int j = 0; j < summary [i].objectivesDone.Length; j++) {
				json += ""+summary [i].objectivesDone[j];
				if(j<summary [i].objectivesDone.Length-1)
					json += ",";
			}
			json += "]}";
			if (i < summary.Count - 1)
				json += ",";;
		}
		json += "]\n}";

		return json;
	}

	public void SetPlayerData(JSONNode N){
		level = N ["level"].AsInt;
		resources = N ["resources"].AsInt;
		toolsNumber = N ["toolsNumber"].AsInt;
		for (int i = 0; i < N ["summary"].Count; i++) {
			Level l = new Level ();
			l.levelNumber = N ["summary"] [i] ["levelNumber"].AsInt;
			for (int j = 0; j < N ["summary"] [i] ["objectivesDone"].Count; j++) {
				l.objectivesDone [j] = N ["summary"] [i] ["objectivesDone"] [j].AsBool;
			}
			summary.Add (l);
		}
	}
}
