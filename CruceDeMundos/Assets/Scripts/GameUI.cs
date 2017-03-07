using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public ResourcesProgress resourcesProgress;

	public GameObject ingameUI;
	public GameObject levelBanner;
	public Text levelNro;
	public Text levelTitle;
	public GameObject mision;
	public GameObject tools;
	public GameObject dialog;
	public GameObject startLevelBanner;
	public GameObject winLevelBanner;
	public GameObject timeOutBanner;
	public GameObject toolsLoseBanner;
	public GameObject siguienteBanner;
	public GameObject iniciarBanner;
	public GameObject tutorial;
	public GameObject summary;
	public TimeProgress timeprogress;

	public Text[] objectives;

	public GameObject[] objectives_summary;

	public GameObject stars2;
	public GameObject stars3;
	public Text summaryTitle;

	// Use this for initialization
	void Start () {
		levelNro.text = "Nivel "+Data.Instance.playerData.level;
		levelTitle.text = Game.Instance.levelManager.leveldata.title;

		resourcesProgress.OnRefreshResources (Data.Instance.playerData.resources);

		//Debug.Log (level.number);
		for (int i = 0; i < objectives.Length; i++) {
			if(i<Game.Instance.levelManager.leveldata.objectives.Count)
				objectives [i].text = Game.Instance.levelManager.leveldata.objectives [i];
			if (objectives [i].text != "") {
				objectives_summary [i].SetActive (true);
				objectives_summary [i].transform.Find ("Text").gameObject.GetComponent<Text> ().text = objectives [i].text;
				if (i == 2)
					stars3.SetActive (true);
			} else if (i == 2) {
				stars2.SetActive (true);
			}
		}

		ingameUI.SetActive (false);
		levelBanner.SetActive (false);

		Events.GameIntro += GameIntro;
		Events.GameMision += GameMision;
		Events.GameTools += GameTools;
		Events.GameHint += GameHint;
		Events.GameReady += GameReady;
		Events.GameActive += GameActive;
		Events.GameDialog += GameDialog;

		Events.StartGame += StartGame;

		Events.OnObjectiveDone += OnObjectiveDone;
		Events.OnTimeOut += OnTimeOut;
		Events.OnToolsLose += OnToolsLose;

	}

	void OnDestroy(){
		Events.GameIntro -= GameIntro;
		Events.GameMision -= GameMision;
		Events.GameTools -= GameTools;
		Events.GameHint -= GameHint;
		Events.GameReady -= GameReady;
		Events.GameActive -= GameActive;
		Events.GameDialog -= GameDialog;

		Events.StartGame -= StartGame;

		Events.OnObjectiveDone -= OnObjectiveDone;
		Events.OnTimeOut -= OnTimeOut;
		Events.OnToolsLose -= OnToolsLose;
	}

	void GameIntro(){		
		levelBanner.SetActive (true);
		Invoke ("HideLevel", 2);
	}

	void HideLevel(){
		siguienteBanner.SetActive (true);
		if (Game.Instance.levelManager.leveldata.isTuto)
			tutorial.SetActive (true);
		levelBanner.SetActive (false);
	}

	void GameMision(){
		siguienteBanner.SetActive (false);
		if (Game.Instance.levelManager.leveldata.isTuto)
			tutorial.SetActive (false);
		mision.SetActive (true);
	}

	void GameTools(){
		mision.SetActive (false);
		tools.SetActive (true);
	}

	void GameHint(){		
		tools.SetActive (false);
		dialog.SetActive (true);
	}

	void GameActive(){
		dialog.SetActive (false);
		ingameUI.SetActive (true);
	}

	void GameDialog(){		
		ingameUI.SetActive (false);
		dialog.SetActive (true);
	}

	void GameReady(){
		dialog.SetActive (false);
		iniciarBanner.SetActive (true);
	}

	void StartGame(){
		iniciarBanner.SetActive (false);
		startLevelBanner.SetActive (true);
		Invoke ("HideStartGame", 1);
	}

	void HideStartGame(){
		startLevelBanner.SetActive (false);
		Events.GameActive ();
		Game.Instance.gameManager.state = GameManager.states.ACTIVE;
	}

	void OnObjectiveDone(){
		Game.Instance.levelManager.CheckTimeObjective (timeprogress.time);
		ingameUI.SetActive (false);
		//winLevelBanner.SetActive (true);
		summary.SetActive (true);
		summaryTitle.text = "¡MISIÓN CUMPLIDA!";
		Game.Instance.levelManager.objectivesDone [0] = true;
		Data.Instance.playerData.resources += Game.Instance.levelManager.leveldata.resourceWin;
		Events.OnRefreshResources (Data.Instance.playerData.resources);
		Game.Instance.gameManager.state = GameManager.states.WIN;
		SetSummary ();
		//Invoke ("HideStarGame", 1);
	}

	void OnTimeOut(){
		ingameUI.SetActive (false);
		//timeOutBanner.SetActive (true);
		summary.SetActive (true);
		summaryTitle.text = "SE ACABÓ EL TIEMPO, INTENTALO NUEVAMENTE";
		Game.Instance.levelManager.objectivesDone [0] = false;
		Game.Instance.gameManager.state = GameManager.states.LOSE;
		SetSummary ();
	}

	void OnToolsLose(){
		ingameUI.SetActive (false);
		//toolsLoseBanner.SetActive (true);
		summary.SetActive (true);
		summaryTitle.text = "TE QUEDASTE SIN HERRAMIENTAS, INTENTALO NUEVAMENTE";
		Game.Instance.levelManager.objectivesDone [0] = false;
		Game.Instance.gameManager.state = GameManager.states.LOSE;
		SetSummary ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetSummary(){
		for (int i = 0; i < Game.Instance.levelManager.objectivesDone.Length; i++) {
			objectives_summary [i].transform.Find ("done").gameObject.SetActive (Game.Instance.levelManager.objectivesDone[i]);
			objectives_summary [i].transform.Find ("undone").gameObject.SetActive (!Game.Instance.levelManager.objectivesDone[i]);
			if(i<2)stars2.transform.Find ("star_"+i).gameObject.SetActive (Game.Instance.levelManager.objectivesDone[i]);
			stars3.transform.Find ("star_"+i).gameObject.SetActive (Game.Instance.levelManager.objectivesDone[i]);
		}
		Data.Instance.playerData.SetSummary ();
	}
}