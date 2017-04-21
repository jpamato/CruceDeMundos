using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;

public class AvatarData : MonoBehaviour {

	public int pielIndex;
	public int caraIndex;
	public int torsoIndex;
	public int piernasIndex;
	public int zapatoIndex;
	public int estadoIndex;
	public Texture2D selfie;
	public int selfie_w;
	public int selfie_h;

	public string[] estados;

	public string folder = "img";
	public string filename = "selfie.png";

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
		LoadSelfieImg ();
	}


	public void CaptureSelfie(RenderTexture rt){						
		selfie = new Texture2D (rt.width, rt.height);
		RenderTexture.active = rt;
		selfie.ReadPixels (new Rect (0, 0, rt.width, rt.height), 0, 0);
		selfie.Apply ();

		byte[] bytes = selfie.EncodeToPNG();
		string filename = Data.Instance.GetFullPathByFolder("img", "selfie.png");
		System.IO.File.WriteAllBytes(filename, bytes);
		Debug.Log(string.Format("Took screenshot to: {0}", filename));
	}

	void LoadSelfieImg(){
		Texture2D tex = null;
		byte[] fileData;

		string filename = Data.Instance.GetFullPathByFolder("img", "selfie.png");

		if (File.Exists(filename))     {
			fileData = File.ReadAllBytes(filename);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
			selfie = tex;
		}

	}
}
