using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;

public class DialogData : MonoBehaviour {

	public bool sendDialogs2Database;
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
		public GameObject visualization;
		public int lastEmoVal;
		public int globalEmoVal;
		public List<LevelInfo> levelsInfo;

		[Serializable]
		public class LevelInfo{
			public int level;
			public int emoval;
			public int goTo;
			public string lastExpre;
			public Dialog.dType dtype;
		}

		public void ResetLevel(int level){
			LevelInfo l = levelsInfo.Find (x => x.level == level);
			if (l != null) {
				l.emoval = 0;
				l.goTo = 0;
			}
		}

		public void ResetType(int level, Dialog.dType dtype){
			LevelInfo l = levelsInfo.Find (x => x.level == level && x.dtype == dtype);
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

	public void ResetHintAtLevel(int level){
		foreach (DialogCharacter dch in dialogCharacters) {
			dch.ResetType (level, Dialog.dType.AUTOEVAL);
			dch.ResetType (level, Dialog.dType.COLLAB);
		}
	}

	IEnumerator Import(string file){

		/*string filePath = "Dialogs/" + file.Replace (".json", "");
		TextAsset text = Resources.Load<TextAsset> (filePath);*/

		WWW www = new WWW("file://" + file);
		yield return www;
		string text = www.text;

		//Debug.Log (text);
		var N = JSON.Parse (text);

		Dialog d = new Dialog ();
		d.name = N ["name"];
		d.level = N ["level"].AsInt;
		d.dialogType = CastDialogType(N["type"]);
		d.initial = N ["initial"] != null ? N ["initial"].AsBool : false;
		d.dialogTree = new Dialog.DialogTree[N ["dialogTree"].Count];
		for (int i = 0; i < d.dialogTree.Length; i++) {
			d.dialogTree[i] = new Dialog.DialogTree();
			d.dialogTree [i].index = N ["dialogTree"] [i] ["index"].AsInt;
			d.dialogTree [i].moods = new Dialog.Mood[N ["dialogTree"] [i] ["moods"].Count];
			for (int j = 0; j < d.dialogTree [i].moods.Length; j++) {
				d.dialogTree [i].moods [j] = new Dialog.Mood ();
				d.dialogTree [i].moods [j].mType = CastMoodType (N ["dialogTree"] [i] ["moods"] [j] ["mood"]);
				d.dialogTree [i].moods [j].prompt = N ["dialogTree"] [i] ["moods"] [j] ["prompt"];
				if (N ["dialogTree"] [i] ["moods"] [j] ["expre"] != null)
					d.dialogTree [i].moods [j].expre = N ["dialogTree"] [i] ["moods"] [j] ["expre"];
				else
					d.dialogTree [i].moods [j].expre = N ["dialogTree"] [i] ["moods"] [j] ["expre"] = "";
				d.dialogTree [i].moods [j].replies = new Dialog.Reply[N ["dialogTree"] [i] ["moods"] [j] ["replies"].Count];
				for (int k = 0; k < d.dialogTree [i].moods [j].replies.Length; k++) {
					d.dialogTree [i].moods [j].replies [k] = new Dialog.Reply ();
					d.dialogTree [i].moods [j].replies [k].emoVal = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["emoVal"].AsInt;
					d.dialogTree [i].moods [j].replies [k].exit = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["exit"].AsBool;
					d.dialogTree [i].moods [j].replies [k].goTo = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["goTo"].AsInt;
					d.dialogTree [i].moods [j].replies [k].text = N ["dialogTree"] [i] ["moods"] [j] ["replies"][k]["text"];
					
					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["rType"] != null)
						d.dialogTree [i].moods [j].replies [k].replyType = CastReplyType(N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["rType"]);
					else
						d.dialogTree [i].moods [j].replies [k].replyType = Dialog.Reply.rType.NARRATIVO;
				
					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["rSType"] != null)
						d.dialogTree [i].moods [j].replies [k].replySubType = CastReplySubType(N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["rSType"]);
					else
						d.dialogTree [i].moods [j].replies [k].replySubType = Dialog.Reply.rSubType.NARRATIVO;

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["indicVal"] != null)
						d.dialogTree [i].moods [j].replies [k].indicadorVal = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["indicVal"];
					else
						d.dialogTree [i].moods [j].replies [k].indicadorVal ="";
				
					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["resources"] != null)
						d.dialogTree [i].moods [j].replies [k].resources = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["resources"].AsInt;
					else
						d.dialogTree [i].moods [j].replies [k].resources = 0;

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["fireCharge"] != null)
						d.dialogTree [i].moods [j].replies [k].fireCharge = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["fireCharge"].AsInt;
					else
						d.dialogTree [i].moods [j].replies [k].fireCharge = 0;

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["portalCharge"] != null)
						d.dialogTree [i].moods [j].replies [k].portalCharge = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["portalCharge"].AsInt;
					else
						d.dialogTree [i].moods [j].replies [k].portalCharge = 0;

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["pollutionCharge"] != null)
						d.dialogTree [i].moods [j].replies [k].pollutionCharge = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["pollutionCharge"].AsInt;
					else
							d.dialogTree [i].moods [j].replies [k].pollutionCharge = 0;

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["tool"] != null) {
						d.dialogTree [i].moods [j].replies [k].tool = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["tool"];
					} else {
						d.dialogTree [i].moods [j].replies [k].tool = "";
					}

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["objective"] != null) {
						d.dialogTree [i].moods [j].replies [k].objective = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["objective"].AsBool;
					} else {
						d.dialogTree [i].moods [j].replies [k].objective = false;
					}

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["dialog"] != null) {
						d.dialogTree [i].moods [j].replies [k].dialog = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["dialog"];
					} else {
						d.dialogTree [i].moods [j].replies [k].dialog = "";
					}

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["levelEnd"] != null) {
						d.dialogTree [i].moods [j].replies [k].levelEndDialog = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["levelEnd"].AsBool;
					} else {
						d.dialogTree [i].moods [j].replies [k].levelEndDialog = false;
					}

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["move"] != null) {
						d.dialogTree [i].moods [j].replies [k].move = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["move"].AsInt;
					} else {
						d.dialogTree [i].moods [j].replies [k].move = -1;
					}

					if (N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["block"] != null) {
						d.dialogTree [i].moods [j].replies [k].block = N ["dialogTree"] [i] ["moods"] [j] ["replies"] [k] ["block"].AsInt;
					} else {
						d.dialogTree [i].moods [j].replies [k].block = 0;
					}
					if (sendDialogs2Database) {
						Debug.Log ("aca");
						StartCoroutine(Data.Instance.dataController.AddDialog (d.name, d.level, d.dialogTree [i].index, d.dialogType.ToString (),
		d.dialogTree [i].moods [j].mType.ToString(), d.dialogTree [i].moods [j].prompt, k, d.dialogTree [i].moods [j].replies [k].text));
					}
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
			AUTOEVAL,
			NARRATIVE,
			COLLAB,
			ET,			
			HUMAN			
		}
		public string name;
		public int level;
		public dType dialogType;
		public bool initial;
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
			public string expre;
			public string prompt;
			public Reply[] replies;

		}
		[Serializable]
		public class Reply{
			public int emoVal;
			public bool exit;
			public int goTo;
			public string text;
			public rType replyType;
			public rSubType replySubType;
			public string indicadorVal;
			public int resources;
			public int fireCharge;
			public int portalCharge;
			public int pollutionCharge;
			public string tool;
			public bool objective;
			public string dialog;
			public bool levelEndDialog;
			public int move;
			public int block;

			public enum rType
			{
				NARRATIVO,
			
				ASERTIVIDAD,
				AUTOEFICACIA,			
				COLABORATIVO,		
				EMPATÍA			
			}

			public enum rSubType{
				NARRATIVO,

				//asertividad
				PROACTIVIDAD,
				INTERÉS,
				PEDIDO,
				NEGARSE,

				//autoeficacia
				JUICIO,
				AUTOPERCEPCIÓN,

				//colaborativo
				COMPARTIR,
				ACEPTAR,
				CONFIANZA,
				TODOS,

				//empatía
				RECONOCIMIENTO,
				ACCIÓN				
			}	
		}

	}

	Dialog.Mood.moodType CastMoodType(string s){
		return (Dialog.Mood.moodType)System.Enum.Parse(typeof(Dialog.Mood.moodType), s.ToUpperInvariant());
		/*if(s.Equals("negative"))
			return Dialog.Mood.moodType.NEGATIVE;
		else if(s.Equals("neutral"))
			return Dialog.Mood.moodType.NEUTRAL;
		else if(s.Equals("positive"))
			return Dialog.Mood.moodType.POSITIVE;	
		else
			return Dialog.Mood.moodType.NEUTRAL;*/
	}

	Dialog.dType CastDialogType(string s){
		return (Dialog.dType)System.Enum.Parse(typeof(Dialog.dType), s.ToUpperInvariant());
		/*if(s.Equals("et"))
			return Dialog.dType.ET;
		else if(s.Equals("hint"))
			return Dialog.dType.HINT;
		else if(s.Equals("human"))
			return Dialog.dType.HUMAN;
		else
			return Dialog.dType.ET;*/
	}


	Dialog.Reply.rType CastReplyType(string s){
		return (Dialog.Reply.rType )System.Enum.Parse(typeof(Dialog.Reply.rType), s.ToUpperInvariant());
	}

	Dialog.Reply.rSubType CastReplySubType(string s){
		return (Dialog.Reply.rSubType )System.Enum.Parse(typeof(Dialog.Reply.rSubType), s.ToUpperInvariant());
	}

	public string GetDialogData(){
		string json = "dialogData:[\n";
		for (int i = 0; i < dialogCharacters.Length; i++) {
			json += "{\n";
			json += "name:"+dialogCharacters[i].name+",\n";
			json += "lastEmoVal:"+dialogCharacters[i].lastEmoVal+",\n";
			json += "globalEmoVal:"+dialogCharacters[i].globalEmoVal+",\n";
			json += "levelsInfo:[";
			for (int j = 0; j < dialogCharacters [i].levelsInfo.Count; j++) {
				json += "{";
				json += "level:" + dialogCharacters [i].levelsInfo [j].level+",";
				json += "emoval:" + dialogCharacters [i].levelsInfo [j].emoval+",";
				json += "goTo:" + dialogCharacters [i].levelsInfo [j].goTo+",";
				json += "dtype:" + dialogCharacters [i].levelsInfo [j].dtype;
				json += "}";
				if(j<dialogCharacters [i].levelsInfo.Count-1)
					json += ",";
			}
			json += "]\n}";
			if(i<dialogCharacters.Length-1)
			json += ",\n";
		}
		json += "]\n";

		return json;
	}

	public void SetDialogData(JSONNode N){
		for (int i = 0; i < N.Count; i++) {
			//DialogCharacter dCh = Array.Find (dialogCharacters, p => p.name == N[i]["name"]);
			dialogCharacters[i].lastEmoVal = N [i] ["lastEmoVal"].AsInt;
			dialogCharacters[i].globalEmoVal = N [i] ["globalEmoVal"].AsInt;
			dialogCharacters [i].levelsInfo.Clear ();
			for (int j = 0; j < N [i] ["levelsInfo"].Count; j++) {
				DialogCharacter.LevelInfo li = new DialogCharacter.LevelInfo ();
				li.level = N [i] ["levelsInfo"] [j] ["level"].AsInt;
				li.emoval = N [i] ["levelsInfo"] [j] ["emoval"].AsInt;
				li.goTo = N [i] ["levelsInfo"] [j] ["goTo"].AsInt;
				li.dtype = (Dialog.dType)System.Enum.Parse(typeof(Dialog.dType), N [i] ["levelsInfo"] [j] ["dtype"]);
				dialogCharacters [i].levelsInfo.Add (li);
			}
		}
	}
}
