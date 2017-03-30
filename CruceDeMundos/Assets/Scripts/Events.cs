using UnityEngine;
using System.Collections;

public static class Events {
	
    public static System.Action<bool> OnPicker = delegate { };
    public static System.Action<string> OnSoundFX = delegate { };

	public static System.Action<int> OnRefreshResources = delegate { };

	public static System.Action GameIntro = delegate { };
	public static System.Action GameMision = delegate { };
	public static System.Action GameTools = delegate { };
	public static System.Action GameAutoeval = delegate { };
	public static System.Action GameReady = delegate { };
	public static System.Action GameActive = delegate { };
	public static System.Action GameDialog = delegate { };
	public static System.Action GameMap = delegate { };

	public static System.Action OnLevelEndDialog = delegate { };

	public static System.Action StartGame = delegate { };

	public static System.Action<string> OnObstacleDestroy = delegate { };
	public static System.Action OnObjectiveDone = delegate { };
	public static System.Action<string> OnDialogObjective = delegate { };
	public static System.Action OnTimeOut = delegate { };
	public static System.Action OnToolsLose = delegate { };

	public static System.Action DialogDone = delegate { };
	public static System.Action<string,int> MoveCharacter = delegate { };
	public static System.Action<string,int> CharacterBlocking = delegate { };
 
	public static System.Action ResetCharacterCollider = delegate { };

	public static System.Action<int, PlayerData.ToolName> OnChargeCollect = delegate { };

	public static System.Action<PlayerData.ToolName> OnAddTool = delegate { };

	public static System.Action OnNewCell = delegate { };
	public static System.Action<VisualCell,VisualCell> OnFalseTrail = delegate { };
	public static System.Action GetVisistedTrail = delegate { };

	public static System.Action<int> OnLevelButtonEnter = delegate { };
	public static System.Action OnLevelButtonExit = delegate { };
}
