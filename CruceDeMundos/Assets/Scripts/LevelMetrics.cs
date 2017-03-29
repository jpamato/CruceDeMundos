﻿using System.Collections;
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

	public int rtBegin;
	public int rtPostTools;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveLevelData(string misions){
		Events.GetVisistedTrail ();

		Data.Instance.SaveLevelData (tools, misions, mapCheck, levelEndTime - levelBeginTime,
			Game.Instance.gameUI.timeprogress.time, map1EndTime - map1BeginTime, objectivesEndTime - objectivesBeginTime, toolsEndTime - toolsBeginTime, trail, rtBegin, rtPostTools);
		
	}
}