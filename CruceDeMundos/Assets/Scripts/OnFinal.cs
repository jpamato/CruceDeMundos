using UnityEngine;
using System.Collections;

public class OnFinal : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	}

	void OnDestroy(){
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		Game.Instance.gameUI.Final ();
		Data.Instance.playerData.level=11;
		Data.Instance.LoadLevel ("Cutscene", 0.2f, 3f, Color.black);
	}
}
