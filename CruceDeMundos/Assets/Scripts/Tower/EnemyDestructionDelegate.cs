using UnityEngine;
using System.Collections;

public class EnemyDestructionDelegate : MonoBehaviour {

	public delegate void EnemyDelegate (GameObject enemy);
	public EnemyDelegate enemyDelegate;

	// Use this for initialization
	void Start () {
	
	}

	void OnDestroy () {
		if (enemyDelegate != null) {
			enemyDelegate (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
