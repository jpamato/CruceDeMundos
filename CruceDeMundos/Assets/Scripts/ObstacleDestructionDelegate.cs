using UnityEngine;
using System.Collections;

public class ObstacleDestructionDelegate : MonoBehaviour {

	public delegate void ObstacleDelegate (GameObject obstacle);
	public ObstacleDelegate obstacleDelegate;

	// Use this for initialization
	void Start () {

	}

	void OnDestroy () {
		if (obstacleDelegate != null) {
			obstacleDelegate (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
