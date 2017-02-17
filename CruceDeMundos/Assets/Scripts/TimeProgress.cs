using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeProgress : MonoBehaviour {

	public Text label;

	public float time;
	void Start () {
		if (Game.Instance.levelManager.leveldata.isVsTime)
			time = Game.Instance.levelManager.leveldata.timeOut;			
	}
	void Update(){
		if (Game.Instance.gameManager.state == GameManager.states.ACTIVE) {
			if (Game.Instance.levelManager.leveldata.isVsTime) {
				time -= Time.deltaTime;
				if (time < 1)
					Events.OnTimeOut ();				
			} else {
				time += Time.deltaTime;
			}
			label.text = Mathf.Floor(time).ToString ();
		}
	}

}
