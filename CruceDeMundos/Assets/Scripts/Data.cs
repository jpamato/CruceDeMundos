using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class Data : MonoBehaviour
{
    const string PREFAB_PATH = "Data";    
    static Data mInstance = null;

	public PlayerData playerData;
	public DialogData dialogData;
	public LevelData levelData;

	public VisualCell lastCell;
	public bool freeTrail = true;

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

		fade = GetComponentInChildren<Fade>();
		//fade.gameObject.SetActive(true);

		DontDestroyOnLoad(this.gameObject);
        
    }
    public void Reset()
    {

    }    
}
