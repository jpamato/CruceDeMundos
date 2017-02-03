using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeProgress : MonoBehaviour {

	public Text label;

	float time;
	void Update(){
		if (Game.Instance.gameManager.state == GameManager.states.ACTIVE) {
			time += Time.deltaTime;
			label.text = Mathf.Floor(time).ToString ();
		}
	}

}
