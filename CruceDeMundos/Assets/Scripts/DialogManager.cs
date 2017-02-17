using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogManager : MonoBehaviour {

	public Image charImage;
	public Text charName;

	public Text charText;
	public Text[] ansText;

	int level;

	DialogData.DialogCharacter character;
	DialogData.DialogCharacter.LevelInfo levelInfo;
	DialogData.Dialog dialog;
	DialogData.Dialog.DialogTree dTree;
	DialogData.Dialog.Mood mood;

	// Use this for initialization
	void Start () {
		level = Data.Instance.playerData.level;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadDialog(string characterName){

		character = Array.Find(Data.Instance.dialogData.dialogCharacters, p => p.name == characterName);
		charImage.sprite = character.sprite;
		charName.text = character.name;

		levelInfo = character.levelsInfo.Find (x => x.level == level);

		if (levelInfo == null) {
			levelInfo = AddNewLevelInfo (character, level);
		}

		dialog = Data.Instance.dialogData.dialogs.Find (x => (x.name == character.name && x.level == level));

		dTree = Array.Find (dialog.dialogTree, p => p.index == levelInfo.goTo);

		mood = Array.Find (dTree.moods, p => (int)p.mType == (levelInfo.emoval + 1));
		charText.text = mood.prompt;
		for (int i = 0; i < mood.replies.Length; i++) {
			ansText [i].text = mood.replies [i].text;
			ansText [i].transform.parent.GetComponent<Button> ().interactable = true;
		}
	}

	DialogData.DialogCharacter.LevelInfo AddNewLevelInfo(DialogData.DialogCharacter character, int level){
		DialogData.DialogCharacter.LevelInfo levelInfo = new DialogData.DialogCharacter.LevelInfo ();
		levelInfo.level = level;
		levelInfo.emoval = 0;
		levelInfo.goTo = 0;
		character.levelsInfo.Add (levelInfo);
		return levelInfo;
	}

	public void ReplySelect(int index){		
		levelInfo.emoval = mood.replies [index].emoVal;
		levelInfo.goTo = mood.replies [index].goTo;

		for (int i = 0; i < ansText.Length; i++) {
			ansText [i].text = "";
			ansText [i].transform.parent.GetComponent<Button> ().interactable = false;
		}

		if (mood.replies [index].resources > 0) {
			Data.Instance.playerData.resources += mood.replies [index].resources;
			Events.OnRefreshResources (Data.Instance.playerData.resources);
		}

		if (mood.replies [index].fireCharge > 0) 
			Events.OnChargeCollect(mood.replies [index].fireCharge, PlayerData.ToolName.Matafuegos);
		if (mood.replies [index].portalCharge > 0) 
			Events.OnChargeCollect(mood.replies [index].portalCharge, PlayerData.ToolName.Restaurador);
		/*if (mood.replies [index].pollutionCharge > 0) 
			Events.OnChargeCollect(mood.replies [index].pollutionCharge, PlayerData.ToolName.A);*/

		if (mood.replies [index].tool != "") {
			if(mood.replies [index].tool.Equals(PlayerData.ToolName.Matafuegos.ToString()))
				Events.OnAddTool(PlayerData.ToolName.Matafuegos);
			else if(mood.replies [index].tool.Equals(PlayerData.ToolName.Restaurador.ToString()))
				Events.OnAddTool(PlayerData.ToolName.Restaurador);
		}

		if (!mood.replies [index].tool.Equals("")) {
			if(mood.replies [index].tool.Equals(PlayerData.ToolName.Matafuegos.ToString())){

			}else if(mood.replies [index].tool.Equals(PlayerData.ToolName.Restaurador.ToString())){

			}
		}
		

		if (mood.replies [index].exit)
			Events.DialogDone ();
		else
			LoadDialog (character.name);

	}

	public void UnlockDialog(string characterName, int level, int goTo){
		character = Array.Find(Data.Instance.dialogData.dialogCharacters, p => p.name == characterName);
		levelInfo = character.levelsInfo.Find (x => x.level == level);
		if (levelInfo == null) {
			levelInfo = AddNewLevelInfo (character, level);
		}
		levelInfo.goTo = goTo;
	}
}
