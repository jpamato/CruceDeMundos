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

		dialog = Data.Instance.dialogData.dialogs.Find (x => (x.name == character.name && x.level == level));

		levelInfo = character.levelsInfo.Find (x => x.level == level);

		if (levelInfo == null) {
			levelInfo = AddNewLevelInfo (character, level, dialog.dialogType);
		}

		dTree = Array.Find (dialog.dialogTree, p => p.index == levelInfo.goTo);

		if (dTree.moods.Length > 1)
			mood = Array.Find (dTree.moods, p => (int)p.mType == (levelInfo.emoval + 1));
		else
			mood = dTree.moods [0];
		
		charText.text = mood.prompt;
		for (int i = 0; i < mood.replies.Length; i++) {
			ansText [i].text = mood.replies [i].text;
			ansText [i].transform.parent.GetComponent<Button> ().interactable = true;
		}
	}

	DialogData.DialogCharacter.LevelInfo AddNewLevelInfo(DialogData.DialogCharacter character, int level, DialogData.Dialog.dType dialogType){
		DialogData.DialogCharacter.LevelInfo levelInfo = new DialogData.DialogCharacter.LevelInfo ();
		levelInfo.level = level;
		levelInfo.emoval = 0;
		levelInfo.goTo = 0;
		levelInfo.dtype = dialogType;
		character.levelsInfo.Add (levelInfo);
		return levelInfo;
	}

	public void ReplySelect(int index){
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click2);
		SendDialogData (character.name, levelInfo.goTo, mood.mType.ToString (), index);
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
			Events.OnChargeCollect (mood.replies [index].fireCharge, PlayerData.ToolName.Matafuegos);
		if (mood.replies [index].portalCharge > 0)
			Events.OnChargeCollect (mood.replies [index].portalCharge, PlayerData.ToolName.Restaurador);
		/*if (mood.replies [index].pollutionCharge > 0) 
			Events.OnChargeCollect(mood.replies [index].pollutionCharge, PlayerData.ToolName.A);*/


		if (mood.replies [index].tool != "") {
			if (mood.replies [index].tool.Equals (PlayerData.ToolName.Matafuegos.ToString ()))
				Events.OnAddTool (PlayerData.ToolName.Matafuegos);
			else if (mood.replies [index].tool.Equals (PlayerData.ToolName.Restaurador.ToString ()))
				Events.OnAddTool (PlayerData.ToolName.Restaurador);
		}

		/*if (!mood.replies [index].tool.Equals("")) {
			if(mood.replies [index].tool.Equals(PlayerData.ToolName.Matafuegos.ToString())){
			}else if(mood.replies [index].tool.Equals(PlayerData.ToolName.Restaurador.ToString())){
			}
		}*/

		if (mood.replies [index].objective) {
			Events.OnDialogObjective (character.name);
		}

		if (mood.replies [index].move > -1)
			Events.MoveCharacter (character.name, mood.replies [index].move);

		if (mood.replies [index].block != 0)
			Events.CharacterBlocking (character.name, mood.replies [index].block);


		if (mood.replies [index].exit){
			Events.DialogDone ();
			if (mood.replies [index].levelEndDialog)
				Events.OnLevelEndDialog ();
		}else{
			if (mood.replies [index].dialog != "") {
				LoadDialog (mood.replies [index].dialog);
			}
			LoadDialog (character.name);
		}

	}

	public void UnlockDialog(string characterName, int level_, int goTo){
		character = Array.Find(Data.Instance.dialogData.dialogCharacters, p => p.name == characterName);
		levelInfo = character.levelsInfo.Find (x => x.level == level_);
		if (levelInfo == null) {
			DialogData.Dialog d = Data.Instance.dialogData.dialogs.Find (x => (x.name == characterName && x.level == level_));
			levelInfo = AddNewLevelInfo (character, level_, d.dialogType);
		}
		levelInfo.goTo = goTo;
	}

	public void SendDialogData(string charName, int index, string mood, int answerId){
		Data.Instance.SaveDialogData (charName, level, index, mood, answerId);
	}
}
