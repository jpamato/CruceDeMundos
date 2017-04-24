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

	public List<string> estados;

	public string folder = "img";
	public string filename = "selfie.png";

	public string PATH = "Selfie/";

	// Use this for initialization
	void Start () {
		estados = new List<string> ();
		#if UNITY_EDITOR
		DirectoryInfo dir = new DirectoryInfo(Directory.GetParent(Application.dataPath).FullName+PATH);
		#elif UNITY_STANDALONE_WIN
		DirectoryInfo dir = new DirectoryInfo(Directory.GetParent(Application.dataPath).FullName+PATH);
		#elif UNITY_STANDALONE_OSX
		DirectoryInfo dir = new DirectoryInfo(Directory.GetParent(Directory.GetParent(Application.dataPath).FullName).FullName+PATH);
		#endif

		FileInfo[] info = dir.GetFiles("*.json");
		foreach (FileInfo f in info) {
		StartCoroutine(Import(f.FullName));
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator Import(string file){

		/*string filePath = "Dialogs/" + file.Replace (".json", "");
		TextAsset text = Resources.Load<TextAsset> (filePath);*/

		WWW www = new WWW("file://" + file);
		yield return www;
		string text = www.text;

		//Debug.Log (text);
		var N = JSON.Parse (text);

		for (int i = 0; i < N["estados"].Count; i++) {
			estados.Add (N["estados"][i]);
		}

	}

	public void SaveAvatarData(){
		string json = "{\n";
		json += "pielIndex:"+pielIndex+",\n";
		json += "caraIndex:"+caraIndex+",\n";
		json += "torsoIndex:"+torsoIndex+",\n";
		json += "piernasIndex:"+piernasIndex+",\n";
		json += "zapatoIndex:"+zapatoIndex+",\n";
		json += "estadoIndex:"+estadoIndex+"\n}";
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
			estadoIndex = N ["estadoIndex"].AsInt;
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
