using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {

	public ResourcesProgress resourcesProgress;

	public GameObject ingameUI;
	public GameObject levelBanner;
	public Text levelNro;
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

	public Text objective1;
	public Text objective2;

	// Use this for initialization
	void Start () {
		levelNro.text = Data.Instance.playerData.level+"";

		resourcesProgress.OnRefreshResources (Data.Instance.playerData.resources);

		//Debug.Log (level.number);

		objective1.text = Game.Instance.levelManager.leveldata.objective1;
		objective2.text = Game.Instance.levelManager.leveldata.objective2;

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
		ingameUI.SetActive (false);
		winLevelBanner.SetActive (true);
		Data.Instance.playerData.resources += Game.Instance.levelManager.leveldata.resourceWin;
		Events.OnRefreshResources (Data.Instance.playerData.resources);
		Game.Instance.gameManager.state = GameManager.states.WIN;
		//Invoke ("HideStarGame", 1);
	}

	void OnTimeOut(){
		ingameUI.SetActive (false);
		timeOutBanner.SetActive (true);
		Game.Instance.gameManager.state = GameManager.states.LOSE;
	}

	void OnToolsLose(){
		ingameUI.SetActive (false);
		toolsLoseBanner.SetActive (true);
		Game.Instance.gameManager.state = GameManager.states.LOSE;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}