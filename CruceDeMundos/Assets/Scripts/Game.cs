using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

	private states lastState;
	public states state;
	public GameUI gameUI;
	public GameManager gameManager;
	public DialogManager dialogManager;
	public CharacterManager characterManager;
	public LevelManager levelManager;
	public ToolsManager toolsManager;
	public PathFinder pathfinder;
	public PathFinder2 pathfinder2;
	public TraceManager traceManager;
	public LevelMetrics levelMetrics;
	public IngameMusic ingameMusic;
	public IngameSfx ingameSfx;

	public GameObject fade;
	[HideInInspector]
	public float globalGlow;

	public enum states
	{
		PAUSED,
		PLAYING,
		ENDED
	}

	static Game mInstance = null;

	public static Game Instance
	{
		get
		{
			if (mInstance == null)
			{
				// Debug.LogError("Algo llama a Game antes de inicializarse");
			}
			return mInstance;
		}
	}
	void Awake () {
		mInstance = this;
		traceManager = GetComponent<TraceManager> ();
	}
	void Start  ()
	{

		/*Events.OnGamePaused += OnGamePaused;
		Events.OnLevelComplete += OnLevelComplete;
		Events.StartGame += StartGame;
		Events.OnGameOver += OnGameOver;
		Events.OnGameRestart += OnGameRestart;*/

		gameManager = GetComponent<GameManager>();
		//gameManager.Init();
		dialogManager = GetComponent<DialogManager>();
		levelManager = GetComponent<LevelManager>();
		toolsManager = GetComponent<ToolsManager>();
		pathfinder = GetComponent<PathFinder>();
		pathfinder2 = GetComponent<PathFinder2>();

		levelMetrics = GetComponent<LevelMetrics>();

		ingameMusic = GetComponentInChildren<IngameMusic> ();
		ingameSfx = GetComponentInChildren<IngameSfx> ();


		FadeIn(2f,true);
		/*ui.Init();
		OnGamePaused(false);*/
	}
	void OnDestroy()
	{
		/*Events.OnGamePaused -= OnGamePaused;
		Events.StartGame -= StartGame;
		Events.OnLevelComplete -= OnLevelComplete;
		Events.OnGameOver -= OnGameOver;
		Events.OnGameRestart -= OnGameRestart;*/
	}
	void StartGame()
	{
		state = states.PLAYING;
	}
	void OnLevelComplete()
	{
		state = states.PAUSED;
	}
	void OnGameOver()
	{
		state = states.PAUSED;       
	}

	public void NextLevel()
	{	
		OnGamePaused(false);
		Data.Instance.playerData.level++;

		if (Data.Instance.playerData.level == 4 || Data.Instance.playerData.level == 8 || Data.Instance.playerData.level == 10) 
			Data.Instance.LoadLevel ("Cutscene", 1f, 3f, Color.black);
		else if (Data.Instance.playerData.level > 10) {
			/*Data.Instance.dialogData.ResetAllAtLevel (1);
			Data.Instance.playerData.level = 1;
			Data.Instance.playerData.resources = 50;
			Data.Instance.playerData.toolsNumber = 1;*/
			Data.Instance.LoadLevel("LevelMap");
		}else
			Data.Instance.LoadLevel("Game");
	}

	public void Replay()
	{	
		OnGamePaused(false);
		Data.Instance.LoadLevel("Game");
	}

	public void GiveUp()
	{	
		OnGamePaused(false);
		if (Game.Instance.levelManager.leveldata.isImposible)
			Game.Instance.levelMetrics.saltearNivel = 1;

		Data.Instance.playerData.UnlockNext ();
		Game.Instance.levelMetrics.SaveLevelData ("");
		Data.Instance.playerData.SetSummary ();
		Data.Instance.SaveGameData ();

		Data.Instance.playerData.level++;
		Data.Instance.LoadLevel("Game");
	}

	public void LevelMap()
	{	
		if (gameManager.state == GameManager.states.MAP) {
			Game.Instance.levelMetrics.levelEndTime = Time.realtimeSinceStartup;
			Data.Instance.LoadGameData ();
		}
		OnGamePaused(false);
		Data.Instance.LoadLevel("LevelMap");
	}

	void OnGamePaused(bool paused)
	{
		if (paused)
		{
			lastState = state;
			Time.timeScale = 0;
			state = states.PAUSED;
		}
		else
		{
			state = lastState;
			Time.timeScale = 1;
		}
	}

	public void FadeIn(float seconds, bool pingpong){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeIn = f.GetComponent<Fade> ();

		fadeIn.OnLoopMethod = () => {
			globalGlow = Mathf.Lerp (0.3f, 1f, fadeIn.time);
		};
		fadeIn.OnEndMethod = () => {
			if(pingpong)
				FadeOut(seconds,pingpong);
			fadeIn.Destroy();
		};
		fadeIn.StartFadeIn (seconds);
	}

	public void FadeOut(float seconds, bool pingpong){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeOut = f.GetComponent<Fade> ();

		fadeOut.OnLoopMethod = () => {
			globalGlow = Mathf.Lerp (0.3f, 1f, fadeOut.time);		
		};
		fadeOut.OnEndMethod = () => {
			if(pingpong)
				FadeIn(seconds,pingpong);
			fadeOut.Destroy();
		};
		fadeOut.StartFadeOut (seconds);
	}
}
