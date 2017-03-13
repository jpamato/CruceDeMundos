using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectivesOver : MonoBehaviour {

	public GameObject go;
	public Text title;
	public Text[] objectives;
	public LevelData.Level leveldata;

	bool over;

	// Use this for initialization
	void Start () {
		Events.OnLevelButtonEnter += OnLevelButtonEnter;
		Events.OnLevelButtonExit += OnLevelButtonExit;
	}

	void OnDestroy(){
		Events.OnLevelButtonEnter -= OnLevelButtonEnter;
		Events.OnLevelButtonExit -= OnLevelButtonExit;
	}

	void OnLevelButtonEnter(int levelNumber){		
		go.SetActive (true);

		title.text = "Objetivos Nivel " + levelNumber;

		leveldata = Data.Instance.levelData.levels.Find (x => x.levelNumber == levelNumber);
		if(leveldata!=null){
			for (int i = 0; i < objectives.Length; i++){
				if (i < leveldata.objectives.Count)
					objectives [i].text = leveldata.objectives [i];
				else
					objectives [i].text = "";
			}
				
		}
	}

	void OnLevelButtonExit(){
		go.SetActive (false);
	}

	public void Reset(){
		Data.Instance.Reset ();
	}
}
