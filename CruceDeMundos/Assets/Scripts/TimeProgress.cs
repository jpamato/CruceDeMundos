using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeProgress : MonoBehaviour {

	public Text label;

	public float time;

	float minFactor;
	bool alarm;
	void Start () {
		if (Game.Instance.levelManager.leveldata.isVsTime)
			time = Game.Instance.levelManager.leveldata.timeOut;			

		minFactor = 1f / 60;
	}
	void Update(){
		if (Game.Instance.gameManager.state == GameManager.states.ACTIVE || Game.Instance.gameManager.state == GameManager.states.MAP) {
			if (Game.Instance.levelManager.leveldata.isVsTime) {
				time -= Time.deltaTime;
				if (time < 10f && !alarm) {
					Game.Instance.ingameSfx.PlaySfx (Game.Instance.ingameSfx.timeAlarm);
					alarm = true;
				}
				if (time < 1)
					Events.OnTimeOut ();				
			} else {
				time += Time.deltaTime;
			}

			int min = (int)(time * minFactor);

			string sec = ((int)time - min * 60).ToString("00");

			label.text = min+":"+sec;
		}
	}

}
