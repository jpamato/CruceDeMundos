using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class DialogData : MonoBehaviour {

	public DialogCharacter[] dialogCharacters;
	public List<Dialog> dialogs;

	public string PATH = "/Resources/Dialogs";

	void Start () {
		dialogs = new List<Dialog> ();
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
		//foreach (string file in System.IO.Directory.GetFiles(Application.dataPath+PATH)){}
	}

	[Serializable]
	public class DialogCharacter
	{
		public string name;
		public Sprite sprite;
		public int lastEmoVal;
		public int globalEmoVal;
		public List<LevelInfo> levelsInfo;

		[Serializable]
		public class LevelInfo{
			public int level;
			public int emoval;
			public int goTo;
		}

		public void ResetLevel(int level){
			LevelInfo l = levelsInfo.Find (x => x.level == level);
			if (l != null) {
				l.emoval = 0;
				l.goTo = 0;
			}
		}
	}

	public void ResetAllAtLevel(int level){
		foreach (DialogCharacter dch in dialogCharacters)
			dch.ResetLevel (level);
	}

	IEnumerator Import(string file){

		/*string filePath = "Dialogs/" + file.Replace (".json", "");
		TextAsset text = Resources.Load<TextAsset> (filePath);*/

		WWW www = new WWW("file://" + file);
		yield return www;
		String text = www.text;

		//Debug.Log (text);
		var N = JSON.Parse (text);

		Dialog d = new Dialog ();
		d.name = N ["name"];
		d.level = N ["level"].AsInt;
		d.dialogType = castDialogType(N["type"]);
		d.dialogTree = new Dialog.DialogTree[N ["dialogTree"].Count];
		for (int i = 0; i < d.dialogTree.Length; i++) {
			d.dialogTree[i] = new Dialog.DialogTree();
			d.dialogTree [i].index = N ["dialogTree"] [i] ["index"].AsInt;
			d.dialogTree [i].moods = new Dialog.Mood[N ["dialogTree"] [i] ["moods"].Count];
			for (int j = 0; j < d.dialogTree [i].moods.Length; j++) {
				d.dialogTree [i].moods [j] = new Dialog.Mood ();
				d.dialogTree [i].moods [j].mType = castMoodType (N ["dialogTree"] [i] ["moods"] [j] ["mood"]);
				d.dialogTree [i].moods [j].prompt = N ["dialogTree"] [i] ["moods"] [j] ["prompt"];
				d.dialogTree [i].moods [j].replies = new Dialog.Reply[N ["dialogTree"] [i] ["moods"] [j] ["replies"].Count];
				for (int k = 0; k < d.dialogTree [i].moods [j].replies.Length; k++) {
					d.dialogTree [i].moods [j].replies [k] = new Dialog.Reply ();
					d.dialogTree [i].moods [j].replies [k].emoVal = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["emoVal"].AsInt;
					d.dialogTree [i].moods [j].replies [k].exit = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["exit"].AsBool;
					d.dialogTree [i].moods [j].replies [k].goTo = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["goTo"].AsInt;
					d.dialogTree [i].moods [j].replies [k].text = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["text"];
					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["resources"] != null)
						d.dialogTree [i].moods [j].replies [k].resources = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["resources"].AsInt;
					else
						d.dialogTree [i].moods [j].replies [k].resources = 0;
				}
			}
		}
		dialogs.Add (d);
	}

	[Serializable]
	public class Dialog
	{
		public enum dType
		{
			ET,
			HINT,
			HUMAN
		}
		public string name;
		public int level;
		public dType dialogType;
		public DialogTree[] dialogTree;

		[Serializable]
		public class DialogTree
		{
			public int index;
			public Mood[] moods;

		}
		[Serializable]
		public class Mood
		{
			public enum moodType
			{
				NEGATIVE,
				NEUTRAL,
				POSITIVE
			}
			public moodType mType;
			public string prompt;
			public Reply[] replies;

		}
		[Serializable]
		public class Reply{
			public int emoVal;
			public bool exit;
			public int goTo;
			public string text;
			public int resources;
		}

	}

	Dialog.Mood.moodType castMoodType(string s){		
		if(s.Equals("negative"))
			return Dialog.Mood.moodType.NEGATIVE;
		else if(s.Equals("neutral"))
			return Dialog.Mood.moodType.NEUTRAL;
		else if(s.Equals("positive"))
			return Dialog.Mood.moodType.POSITIVE;	
		else
			return Dialog.Mood.moodType.NEUTRAL;
	}

	Dialog.dType castDialogType(string s){
		if(s.Equals("et"))
			return Dialog.dType.ET;
		else if(s.Equals("hint"))
			return Dialog.dType.HINT;
		else if(s.Equals("human"))
			return Dialog.dType.HUMAN;
		else
			return Dialog.dType.ET;
	}
}
