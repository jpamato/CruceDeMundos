using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class AvatarData : MonoBehaviour {

	public int pielIndex;
	public int caraIndex;
	public int torsoIndex;
	public int piernasIndex;
	public int zapatoIndex;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SaveAvatarData(){
		string json = "{\n";
		json += "pielIndex:"+pielIndex+",\n";
		json += "caraIndex:"+caraIndex+",\n";
		json += "torsoIndex:"+torsoIndex+",\n";
		json += "piernasIndex:"+piernasIndex+",\n";
		json += "zapatoIndex:"+zapatoIndex+"\n}";
		PlayerPrefs.SetString ("AvatarData",json);
	}

	public void LoadAvatarData(){
		string json = PlayerPrefs.GetString ("AvatarData");
		if (!json.Equals ("")) {
			var N = JSON.Parse (json);
			pielIndex = N ["pielIndex"].AsInt;
			caraIndex = N ["caraIndex"].AsInt;
			torsoIndex = N ["torsoIndex"].AsInt;
			piernasIndex = N ["piernasIndex"].AsInt;
			zapatoIndex = N ["zapatoIndex"].AsInt;
		}
	}
}
