using UnityEngine;
using System.Collections;

public class WaterShot : MonoBehaviour {

	public bool active;
	ShootObstacle shotObstacle;
	ToolData td; 

	// Use this for initialization
	void Start () {
		shotObstacle = GetComponent<ShootObstacle> ();
		td = GetComponent<ToolData> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (td.currentLevel.visualization != null) {
			if (!active) {
				if (shotObstacle.shooting) {
					td.currentLevel.visualization.GetComponent<WaterStream> ().fire.SetActive (true);
					active = true;
				}
			} else {
				if (!shotObstacle.shooting) {
					td.currentLevel.visualization.GetComponent<WaterStream> ().fire.SetActive (false);
					active = false;
				}
			}
		}
	}
}
