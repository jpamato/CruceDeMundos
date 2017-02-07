using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeProgress : MonoBehaviour {

	public Text label;

	float time;
	void Start () {
		time = Game.Instance.gameManager.time;
	}
	void Update(){
		if (Game.Instance.gameManager.state == GameManager.states.ACTIVE) {
			time -= Time.deltaTime;
			label.text = Mathf.Floor(time).ToString ();
			if (time < 1) {				
				Events.OnTimeOut ();
			}
		}
	}

}
