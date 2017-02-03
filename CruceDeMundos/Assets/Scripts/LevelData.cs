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
		public int number;
		public string objective1;
		public string objective2;
	}
}
