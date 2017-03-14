using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

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

	public void LoadLevel(string aLevelName)
    {        
        LoadLevel(aLevelName, 0.01f, 0.01f, Color.black);
    }
    public void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
    {
        //Application.LoadLevel(aLevelName);
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

		fade = GetComponentInChildren<Fade>();
		//fade.gameObject.SetActive(true);

		LoadUserData();
		LoadGameData();
		//Reset();

		DontDestroyOnLoad(this.gameObject);
        
    }

	public void SaveUserData(){		
		StartCoroutine(dataController.CreateUserRoutine(userId,SystemInfo.deviceUniqueIdentifier, userName));
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
		if (!json.Equals("")) {
			var N = JSON.Parse (json);
			playerData.SetPlayerData (N ["playerData"]);
			dialogData.SetDialogData (N ["dialogData"]);
		}
	}

    public void Reset(){
		PlayerPrefs.DeleteAll ();
		Data.Instance.LoadLevel("LevelMap");
    }

	public void SendData(){
		dialogData.ResetHintAtLevel(playerData.level);
	}
}
