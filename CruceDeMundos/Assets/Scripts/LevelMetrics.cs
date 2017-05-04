using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMetrics : MonoBehaviour {

	public int mapCheck;
	public string tools;
	public float levelBeginTime;
	public float levelEndTime;

	public float map1BeginTime;
	public float map1EndTime;

	public float objectivesBeginTime;
	public float objectivesEndTime;

	public float toolsBeginTime;
	public float toolsEndTime;

	public string trail;
	public string deadEnds;

	public int rtBegin;
	public int rtPostTools;

	public int saltearNivel = -1;

	public int portalesCerrados;
	public int fuegosApagados;
	public int polucionEliminada;

	// Use this for initialization
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
		if (tag == ShootObstacle.obstacleType.PORTAL.ToString()) {				
			portalesCerrados++;
		}else if (tag == ShootObstacle.obstacleType.FIRE.ToString()) {				
			fuegosApagados++;
		}else if (tag == ShootObstacle.obstacleType.POLLUTION.ToString()) {				
			polucionEliminada++;
		}
	}

	public void SaveLevelData(string misions){
		Events.GetVisistedTrail ();

		Data.Instance.SaveLevelData (tools, misions, portalesCerrados, fuegosApagados, polucionEliminada, mapCheck, levelEndTime - levelBeginTime,
			Game.Instance.gameUI.timeprogress.time, map1EndTime - map1BeginTime, objectivesEndTime - objectivesBeginTime, toolsEndTime - toolsBeginTime, trail, deadEnds, rtBegin, rtPostTools, saltearNivel);
		
	}
}
