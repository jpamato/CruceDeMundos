using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleData : MonoBehaviour {

	[System.Serializable]
	public class ObstacleLevel {		
		public GameObject visualization;
		public int damage;
		public float fireRate;
		public int health;
	}

	public List<ObstacleLevel> levels;

	private ObstacleLevel currentLevel;

	private AnimSteps animSteps;

	// Use this for initialization
	void Start () {
		animSteps = GetComponent<AnimSteps> ();
		if (animSteps == null) {
			animSteps = currentLevel.visualization.GetComponent<AnimSteps> ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	void OnEnable() {
		CurrentLevel = levels[0];
	}

	//1
	public ObstacleLevel CurrentLevel {
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

	public ObstacleLevel getNextLevel() {
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

	public bool RecibeDamage(float val){
		currentLevel.health -= 1;
		animSteps.SetNextStep (currentLevel.health);
		if (currentLevel.health <= 0)
			return true;
		else
			return false;
	}
}
