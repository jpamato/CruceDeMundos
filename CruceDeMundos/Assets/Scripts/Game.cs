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
	public TraceManager traceManager;

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

	public void OnGameRestart()
	{	
		OnGamePaused(false);
		Data.Instance.dialogData.ResetAllAtLevel (1);
		Data.Instance.LoadLevel("Proto"); 

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
}
