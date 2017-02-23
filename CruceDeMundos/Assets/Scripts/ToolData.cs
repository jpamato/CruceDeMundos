using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToolData : MonoBehaviour {

	[System.Serializable]
	public class ToolLevel {
		public int cost;
		public GameObject visualization;
		public GameObject bullet;
		public float fireRate;
		public float fireLoss;
	}

	public ToolLevel currentLevel;

	public List<ToolLevel> levels;
	public PlayerData.ToolName toolType;



	// Use this for initialization
	void Awake () {
		CurrentLevel = levels[0];
	}

	// Update is called once per frame
	void Update () {

	}

	void OnDisable() {
		currentLevel.visualization.SetActive (false);
	}

	void OnEnable() {		
		currentLevel.visualization.SetActive (true);
	}

	//1
	public ToolLevel CurrentLevel {
		//2
		get {
			return currentLevel;
		}
		//3
		set {
			currentLevel = value;
			int currentLevelIndex = levels.IndexOf(currentLevel);

			GameObject levelVisualization = levels[currentLevelIndex].visualization;
			for (int i = 0; i < levels.Count; i++) {
				if (levelVisualization != null) {
					if (i == currentLevelIndex) {
						levels[i].visualization.SetActive(true);
					} else {
						levels[i].visualization.SetActive(false);
					}
				}
			}
		}
	}

	public ToolLevel getNextLevel() {
		int currentLevelIndex = levels.IndexOf (currentLevel);
		int maxLevelIndex = levels.Count - 1;
		if (currentLevelIndex < maxLevelIndex) {
			return levels[currentLevelIndex+1];
		} else {
			return null;
		}
	}

	public void increaseLevel() {
		int currentLevelIndex = levels.IndexOf(currentLevel);
		if (currentLevelIndex < levels.Count - 1) {
			CurrentLevel = levels[currentLevelIndex + 1];
		}
	}

	public void SetLevel(int currentLevelIndex){	
		if (currentLevelIndex > 0) {
			currentLevel = levels [currentLevelIndex];
			GameObject levelVisualization = levels [currentLevelIndex].visualization;
			for (int i = 0; i < levels.Count; i++) {
				if (levelVisualization != null) {
					if (i == currentLevelIndex) {
						levels [i].visualization.SetActive (true);
					} else {
						levels [i].visualization.SetActive (false);
					}
				}
			}
		}
	}
}
