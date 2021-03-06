﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleData : MonoBehaviour {

	public AudioClip obstacleImpact;
	public AudioClip obstacleDeath;

	public float colliderScaleFactor = 3;

	[System.Serializable]
	public class ObstacleLevel {		
		public GameObject visualization;
		public int damage;
		public float fireRate;
		public int health;
	}

	public List<ObstacleLevel> levels;

	public ObstacleLevel currentLevel;
	private CircleCollider2D obCollider;

	private AnimSteps animSteps;

	// Use this for initialization

	AudioSource source;

	void Awake(){
		obCollider = GetComponent<CircleCollider2D> ();
		CurrentLevel = levels[0];
	}

	void Start () {
		animSteps = GetComponent<AnimSteps> ();
		if (animSteps == null) {
			animSteps = currentLevel.visualization.GetComponent<AnimSteps> ();
		}

		source = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnEnable() {
		
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
						obCollider.radius = colliderScaleFactor * levels [i].visualization.transform.lossyScale.x;
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
		if (currentLevel.health == 0) {
			source.PlayOneShot (obstacleDeath);
			return true;
		} else {
			source.PlayOneShot (obstacleImpact);
			return false;
		}
	}
}
