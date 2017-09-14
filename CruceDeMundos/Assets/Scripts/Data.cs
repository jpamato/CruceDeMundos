using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine.Audio;

public class Data : MonoBehaviour
{
    const string PREFAB_PATH = "Data";    
    static Data mInstance = null;

	public string userId;
	public string userName;
	public PlayerData playerData;
	public DialogData dialogData;
	public LevelData levelData;

	public DataController dataController;

	public AvatarData avatarData;

	public CSDialogData csdialogData;

	public MusicManager musicManager;
	public InterfaceSfx interfaceSfx;
	public AudioMixer audioMaster;
	public bool mute;

	public bool unlockAllLevels;

	private Fade fade;

    public static Data Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<Data>();

                if (mInstance == null)
                {
                    GameObject go = Instantiate(Resources.Load<GameObject>(PREFAB_PATH)) as GameObject;
                    mInstance = go.GetComponent<Data>();
                    go.transform.localPosition = new Vector3(0, 0, 0);
                }
            }
            return mInstance;
        }
    }

	void OnApplicationFocus(bool hasFocus)
	{
		Mute (!hasFocus);
	}

	public void LoadLevel(string aLevelName)
    {        
		musicManager.MusicChange (aLevelName);
        LoadLevel(aLevelName, 0.01f, 0.01f, Color.black);
    }
    public void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        //Application.LoadLevel(aLevelName);
	 	musicManager.MusicChange (aLevelName);
       fade.LoadLevel(aLevelName, aFadeOutTime, aFadeInTime, aColor);

    }
    void Awake()
    {
        
        if (!mInstance)
            mInstance = this;
        //otherwise, if we do, kill this thing
        else
        {
            Destroy(this.gameObject);
            return;
        }

		playerData = GetComponent<PlayerData> ();
		dialogData = GetComponent<DialogData> ();
		levelData = GetComponent<LevelData> ();
		dataController = GetComponent<DataController> ();
		avatarData = GetComponent<AvatarData> ();
		csdialogData = GetComponent<CSDialogData> ();
		musicManager = GetComponent<MusicManager> ();
		interfaceSfx = GetComponentInChildren<InterfaceSfx> ();

		fade = GetComponentInChildren<Fade>();
		//fade.gameObject.SetActive(true);

		LoadUserData();
		LoadGameData();
		avatarData.LoadAvatarData ();
		//Reset();

		DontDestroyOnLoad(this.gameObject);
        
    }

	public void SaveDialogData(string character, int level, int index, string mood, int answerId){		
		StartCoroutine(dataController.SaveDialogData(userId,SystemInfo.deviceUniqueIdentifier, character, level, index, mood, answerId));
	}

	public void SaveLevelData(string tools, string toolsEnd, string misions, int portalDone, int fireDone, int pollutionDone, int mapchecks,
		float levelTime, float gameTime, float mapTime, float misionTime, float toolsTime, string mapTrail, string mapDeadEnds, int rtB, 
		int rtAt, int pCharge, int fCharge, int poCharge, int rtCharge, int giveup){		
		StartCoroutine(dataController.SaveLevelData(userId,SystemInfo.deviceUniqueIdentifier, playerData.level, tools, toolsEnd, misions, portalDone, fireDone, pollutionDone,mapchecks,
			levelTime, gameTime, mapTime, misionTime, toolsTime, mapTrail, mapDeadEnds, rtB, rtAt, playerData.resources, pCharge, fCharge,
			poCharge, rtCharge, giveup));
	}

	public void SaveUserData(){		
		StartCoroutine(dataController.CreateUserRoutine(userId,SystemInfo.deviceUniqueIdentifier, userName));
	}

	public void SaveSelfie(string emoji, string estado){		
		StartCoroutine(dataController.SaveSelfie(userId,SystemInfo.deviceUniqueIdentifier, playerData.level, emoji, estado));
	}

	public void LoadUserData(){
		string json = PlayerPrefs.GetString ("UserData");
		if (!json.Equals ("")) {
			var N = JSON.Parse (json);
			userId = N ["userId"];
			userName = N ["userName"];
		} /*else {
			userId = "";
			userName = "";
		}*/
	}

	public void SaveGameData(){
		SendData ();
		string json = "{";
		json+= playerData.GetPlayerData ()+",";
		json += dialogData.GetDialogData ()+"}";
		PlayerPrefs.SetString ("GameData",json);
	}

	public void LoadGameData(){
		string json = PlayerPrefs.GetString ("GameData");
		if (!json.Equals ("")) {
			var N = JSON.Parse (json);
			playerData.SetPlayerData (N ["playerData"]);
			dialogData.SetDialogData (N ["dialogData"]);
		} else {
			SaveGameData ();
		}
	}

    public void Reset(){
		PlayerPrefs.DeleteAll ();
		Data.Instance.LoadLevel("LevelMap");
    }

	public void SendData(){
		dialogData.ResetHintAtLevel(playerData.level);
	}

	public void Mute(bool m){
		mute = m;
		if (mute)
			audioMaster.SetFloat ("masterVolume", -80f);
		else
			audioMaster.SetFloat ("masterVolume", 0f);
	}

	public string GetFullPathByFolder(string FolderName, string fileName)
	{
		string folder = Path.Combine(Application.persistentDataPath, FolderName);
		if (!Directory.Exists(folder))
			Directory.CreateDirectory(folder);
		return Path.Combine(folder, fileName);
	}

	public void Exit(){
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
		#else
		Application.Quit();
		#endif
	}
}
