using UnityEngine;
using System.Collections;

public class LevelButton : MonoBehaviour {

	public int levelNumber;
	public GameObject locked;
	public GameObject unlocked;
	public GameObject played;
	public GameObject done;
	public GameObject stars2;
	public GameObject stars3;
	public PlayerData.Level.LevelState levelState;

	// Use this for initialization
	void Start () {
		PlayerData.Level level = Data.Instance.playerData.summary.Find (x => x.levelNumber == levelNumber);
		if (level == null) {
			if (levelNumber == 1) {
				unlocked.SetActive (true);
				levelState = PlayerData.Level.LevelState.UNLOCKED;
			} else {
				locked.SetActive (true);
				levelState = PlayerData.Level.LevelState.LOCKED;
			}
		} else {
			if (level.levelState == PlayerData.Level.LevelState.LOCKED) {
				locked.SetActive (true);
				levelState = PlayerData.Level.LevelState.LOCKED;
			}else if (level.levelState == PlayerData.Level.LevelState.UNLOCKED){
				unlocked.SetActive (true);
				levelState = PlayerData.Level.LevelState.UNLOCKED;
			}else if(level.levelState==PlayerData.Level.LevelState.PLAYED){
				played.SetActive (true);
				levelState = PlayerData.Level.LevelState.PLAYED;
			}else if(level.levelState==PlayerData.Level.LevelState.DONE){
				done.SetActive (true);			
				levelState = PlayerData.Level.LevelState.DONE;
			}
		}
		LevelData.Level ldata = Data.Instance.levelData.levels.Find (x => x.levelNumber == levelNumber);
		if (ldata == null) {
			stars3.SetActive (true);
		} else {
			GameObject stars;
			if (ldata.objectives.Count < 3)
				stars = stars2;
			else
				stars = stars3;

				stars.SetActive (true);
				if(levelState==PlayerData.Level.LevelState.PLAYED||levelState==PlayerData.Level.LevelState.DONE){					
					Stars s = stars.GetComponent<Stars> ();
					for (int i = 0; i < ldata.objectives.Count; i++) {						
						if (level.objectivesDone [i]) {
							s.SetStarOn (i);						
						}
					}
				}
		}		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayLevel(){
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Data.Instance.playerData.level = levelNumber;
		Data.Instance.LoadLevel("Game");
	}

	public void OnPointerEnter(){		
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.over);
		Events.OnLevelButtonEnter (levelNumber);
	}

	public void OnPointerExit(){
		Events.OnLevelButtonExit();
	}
}
