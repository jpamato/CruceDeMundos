﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DialogManager : MonoBehaviour {

	public CharacterManager chManager;
	public Image charImage;
	//public Text charName;

	public Text charText;
	public Text[] ansText;

	public Transform buttonsContainer;

	int level;

	DialogData.DialogCharacter character;
	DialogData.DialogCharacter.LevelInfo levelInfo;
	DialogData.Dialog dialog;
	DialogData.Dialog.DialogTree dTree;
	DialogData.Dialog.Mood mood;

	public float dialogBeginTime;

	public float replyWaitTime;

	// Use this for initialization
	void Start () {
		level = Data.Instance.playerData.level;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool LoadInitialDialog(){
		dialog = Data.Instance.dialogData.dialogs.Find (x => (x.initial == true && x.level == level));

		if (dialog != null) {
			character = Array.Find (Data.Instance.dialogData.dialogCharacters, p => p.name == dialog.name);
			//charImage.sprite = character.sprite;
			chManager.SetCharacter (character.visualization);
			//charName.text = character.name;
			LoadDialog ();
			return true;
		} else {
			return false;
		}
	}

	public bool LoadFinalDialog(){
		dialog = Data.Instance.dialogData.dialogs.Find (x => (x.final == true && x.level == level));

		if (dialog != null) {
			character = Array.Find (Data.Instance.dialogData.dialogCharacters, p => p.name == dialog.name);
			//charImage.sprite = character.sprite;
			chManager.SetCharacter (character.visualization);
			//charName.text = character.name;
			LoadDialog ();
			return true;
		} else {
			return false;
		}
	}

	public void LoadDialog(string characterName){

		character = Array.Find(Data.Instance.dialogData.dialogCharacters, p => p.name == characterName);
		//charImage.sprite = character.sprite;
		chManager.SetCharacter(character.visualization);
		//charName.text = character.name;

		dialog = Data.Instance.dialogData.dialogs.Find (x => (x.name == character.name && x.level == level));

		LoadDialog ();
	}

	public void LoadDialog(){
		levelInfo = character.levelsInfo.Find (x => x.level == level);

		if (levelInfo == null) {
			levelInfo = AddNewLevelInfo (character, level, dialog.dialogType);
		}

		dTree = Array.Find (dialog.dialogTree, p => p.index == levelInfo.goTo);

		if (dTree.moods.Length > 1)
			mood = Array.Find (dTree.moods, p => (int)p.mType == (levelInfo.emoval + 1));
		else
			mood = dTree.moods [0];

		if (mood.expre != "") {
			chManager.SetAnimation (mood.expre);
			levelInfo.lastExpre = mood.expre;
		}else if(levelInfo.lastExpre!="")
			chManager.SetAnimation (levelInfo.lastExpre);

		ShuffleChildOrder (buttonsContainer);

		charText.text = mood.prompt.Replace("#Manu",Data.Instance.userName);
		for (int i = 0; i < mood.replies.Length; i++) {
			ansText [i].text = mood.replies [i].text;
			ansText [i].color = new Color (0.8f, 0.8f, 0.8f);
			//ansText [i].transform.parent.GetComponent<Button> ().interactable = true;
		}
		Invoke ("EnableReplies", 4f);

		Game.Instance.ingameSfx.PlaySfx (Game.Instance.ingameSfx.dialog);
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
		SendDialogData (character.name, levelInfo.goTo, mood.mType.ToString (), index, Time.realtimeSinceStartup-dialogBeginTime);
		levelInfo.emoval = mood.replies [index].emoVal;
		levelInfo.goTo = mood.replies [index].goTo;

		for (int i = 0; i < ansText.Length; i++) {
			ansText [i].text = "";
			ansText [i].transform.parent.GetComponent<Button> ().interactable = false;
		}

		if (mood.replies [index].resources != 0) {
			Data.Instance.playerData.resources += mood.replies [index].resources;
			Events.OnRefreshResources (Data.Instance.playerData.resources);
		}

		if (mood.replies [index].fireCharge > 0)
			Events.OnChargeCollect (mood.replies [index].fireCharge, PlayerData.ToolName.Matafuegos);
		if (mood.replies [index].portalCharge > 0)
			Events.OnChargeCollect (mood.replies [index].portalCharge, PlayerData.ToolName.Restaurador);
		if (mood.replies [index].pollutionCharge > 0) 
			Events.OnChargeCollect(mood.replies [index].pollutionCharge, PlayerData.ToolName.Armonizador);


		if (mood.replies [index].tool != "") {
			if (mood.replies [index].tool.Equals (PlayerData.ToolName.Matafuegos.ToString ()))
				Events.OnAddTool (PlayerData.ToolName.Matafuegos);
			else if (mood.replies [index].tool.Equals (PlayerData.ToolName.Restaurador.ToString ()))
				Events.OnAddTool (PlayerData.ToolName.Restaurador);
			else if (mood.replies [index].tool.Equals (PlayerData.ToolName.Armonizador.ToString ()))
				Events.OnAddTool (PlayerData.ToolName.Armonizador);
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

		if (mood.replies [index].dUnlock.goTo > -1)
			UnlockDialog (mood.replies [index].dUnlock.characterName, level, mood.replies [index].dUnlock.goTo);

		if (mood.replies [index].oType != "") {
			GameObject[] go = GameObject.FindGameObjectsWithTag (mood.replies [index].oType);
			if (go != null && go.Length>0) {
				Destroy (go[go.Length-1]);
				Events.OnObstacleDestroy (mood.replies [index].oType);
			}
		}

		if (mood.replies [index].friendDisable != "")
			Game.Instance.toolsManager.DisableFriend(mood.replies [index].friendDisable);
		
		chManager.Close ();
		if (mood.replies [index].exit){
			Events.DialogDone ();
			if (mood.replies [index].levelEndDialog)
				Events.OnLevelEndDialog ();
		}else{
			if (mood.replies [index].dialog != "") {
				LoadDialog (mood.replies [index].dialog);
			} else {
				LoadDialog (character.name);
			}
		}

	}

	void EnableReplies(){
		for (int i = 0; i < mood.replies.Length; i++) {
			ansText [i].color = new Color (0.2f, 0.2f, 0.2f);
			ansText [i].transform.parent.GetComponent<Button> ().interactable = true;
		}
		dialogBeginTime = Time.realtimeSinceStartup;
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

	public void SendDialogData(string charName, int index, string mood, int answerId, float time){
		Data.Instance.SaveDialogData (charName, level, index, mood, answerId, time);
	}

	public void ShuffleChildOrder(Transform container){
		for (int i = 0; i < container.childCount; i++) {			
			Transform t = container.GetChild (i);
			if (UnityEngine.Random.value < 0.3f)
				t.transform.SetAsFirstSibling ();
			else if (UnityEngine.Random.value < 0.6)
				t.transform.SetAsLastSibling ();
		}
	}
}
