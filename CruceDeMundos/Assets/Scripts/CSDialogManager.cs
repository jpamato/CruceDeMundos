using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class CSDialogManager : MonoBehaviour {

	public CharacterManager chManager;
	public GameObject dialogUI;
	public Image charImage;
	//public Text charName;

	public Text charText;
	public Text[] ansText;

	public List<Background> backgrounds;

	public int level;

	CSDialogData.DialogCharacter character;
	CSDialogData.DialogCharacter.LevelInfo levelInfo;
	CSDialogData.Dialog dialog;
	CSDialogData.Dialog.DialogTree dTree;
	CSDialogData.Dialog.Mood mood;

	// Use this for initialization
	void Start () {
		level = Data.Instance.playerData.level;
	}

	// Update is called once per frame
	void Update () {

	}

	[Serializable]
	public class Background
	{
		public string name;
		public GameObject visualization;
	}

	void SelectBackground(string bname){
		foreach (Background b in backgrounds)
			b.visualization.SetActive (bname == b.name);	
	}

	public bool LoadInitialDialog(){
		dialogUI.SetActive (true);
		dialog = Data.Instance.csdialogData.dialogs.Find (x => (x.initial == true && x.level == level));

		if (dialog != null) {
			character = Array.Find (Data.Instance.csdialogData.dialogCharacters, p => p.name == dialog.name);
			//charImage.sprite = character.sprite;
			chManager.SetCharacter (character.visualization,dialog.name=="Manu");
			//charName.text = character.name;
			LoadDialog ();
			return true;
		} else {
			return false;
		}
	}

	public bool LoadFinalDialog(){
		dialog = Data.Instance.csdialogData.dialogs.Find (x => (x.final == true && x.level == level));

		if (dialog != null) {
			character = Array.Find (Data.Instance.csdialogData.dialogCharacters, p => p.name == dialog.name);
			//charImage.sprite = character.sprite;
			chManager.SetCharacter (character.visualization,dialog.name=="Manu");
			//charName.text = character.name;
			LoadDialog ();
			return true;
		} else {
			return false;
		}
	}

	public void LoadDialog(string characterName){

		character = Array.Find(Data.Instance.csdialogData.dialogCharacters, p => p.name == characterName);
		//charImage.sprite = character.sprite;
		chManager.SetCharacter (character.visualization,characterName=="Manu");
		//charName.text = character.name;

		dialog = Data.Instance.csdialogData.dialogs.Find (x => (x.name == character.name && x.level == level));

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

		if(Data.Instance.userName!=null)
		charText.text = mood.prompt.Replace("#Manu",Data.Instance.userName);
		for (int i = 0; i < mood.replies.Length; i++) {
			ansText [i].text = mood.replies [i].text;
			ansText [i].transform.parent.GetComponent<Button> ().interactable = true;
		}

		SelectBackground (mood.background);

		//Game.Instance.ingameSfx.PlaySfx (Game.Instance.ingameSfx.dialog);
	}

	CSDialogData.DialogCharacter.LevelInfo AddNewLevelInfo(CSDialogData.DialogCharacter character, int level, CSDialogData.Dialog.dType dialogType){
		CSDialogData.DialogCharacter.LevelInfo levelInfo = new CSDialogData.DialogCharacter.LevelInfo ();
		levelInfo.level = level;
		levelInfo.emoval = 0;
		levelInfo.goTo = 0;
		levelInfo.dtype = dialogType;
		character.levelsInfo.Add (levelInfo);
		return levelInfo;
	}

	public void ReplySelect(int index){
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click2);
		//SendDialogData (character.name, levelInfo.goTo, mood.mType.ToString (), index);
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
			if (go != null) {
				Destroy (go[go.Length-1]);
				Events.OnObstacleDestroy (mood.replies [index].oType);
			}
		}

		if (mood.replies [index].friendDisable != "")
			Game.Instance.toolsManager.DisableFriend(mood.replies [index].friendDisable);
		chManager.Close ();
		if (mood.replies [index].exit){
			if (mood.replies [index].levelMap) {
				Data.Instance.playerData.level = 1;
				Data.Instance.LoadLevel ("LevelMap", 1f, 0.5f, Color.black);
			}else if (mood.replies [index].blackout) {
				Data.Instance.playerData.level = 11;
				Data.Instance.LoadLevel ("Cutscene", 1f, 3f, Color.black);
			}else
				Data.Instance.LoadLevel ("Game", 1f, 0.5f, Color.black);
		}else{
			if (mood.replies [index].dialog != "") {
				LoadDialog (mood.replies [index].dialog);
			} else {
				LoadDialog (character.name);
			}
		}

	}

	public void UnlockDialog(string characterName, int level_, int goTo){
		character = Array.Find(Data.Instance.csdialogData.dialogCharacters, p => p.name == characterName);
		levelInfo = character.levelsInfo.Find (x => x.level == level_);
		if (levelInfo == null) {
			CSDialogData.Dialog d = Data.Instance.csdialogData.dialogs.Find (x => (x.name == characterName && x.level == level_));
			levelInfo = AddNewLevelInfo (character, level_, d.dialogType);
		}
		levelInfo.goTo = goTo;
	}

	public void SendDialogData(string charName, int index, string mood, int answerId){
		Data.Instance.SaveDialogData (charName, level, index, mood, answerId);
	}
}