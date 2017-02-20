using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleData : MonoBehaviour {

	[System.Serializable]
	public class ObstacleLevel {		
		public GameObject visualization;
		public int damage;
		public float fireRate;
	}

	public List<ObstacleLevel> levels;

	private ObstacleLevel currentLevel;

	private Animation anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
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
		/*Debug.Log (anim ["vortex"].time + "+" +val);
		anim["vortex"].time = val*0.1f+anim["vortex"].time;
		anim["vortex"].speed = 1.0f;
		anim.Play("vortex");
		if (anim ["vortex"].time > 60f) {
			return true;
		} else {
			return false;
		}*/
		Debug.Log (anim ["vortex"].speed);
		return false;
	}
}
