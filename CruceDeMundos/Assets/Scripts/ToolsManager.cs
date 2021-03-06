﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ToolsManager : MonoBehaviour {

	public FriendTool[] friendsTools;
	public List<string> damagingObstacles;

	[System.Serializable]
	public class FriendTool {
		public string name;
		public GameObject friend;
		public string toolName;
		public int toolLevel;
		public bool hasCharge;
	}

	// Use this for initialization
	void Start () {		
		Events.OnChargeCollect += OnChargeCollect;
		Events.OnAddTool += OnAddTool;
	}

	void OnDestroy(){		
		Events.OnChargeCollect -= OnChargeCollect;
		Events.OnAddTool -= OnAddTool;
	}

	void OnChargeCollect(int val, PlayerData.ToolName tType){
		int cant = Array.FindAll (friendsTools, p => p.toolName == tType.ToString()).Length;
		float charge = 1f * val / cant;
		for (int i = 0; i < friendsTools.Length; i++) {
			if (friendsTools[i].toolName == tType.ToString()) {
				friendsTools [i].hasCharge = true;
				HealthBar healthBar = friendsTools[i].friend.GetComponentInChildren<HealthBar>();
				//healthBar.CurrentHealth = Mathf.Min(healthBar.currentHealth+charge, 100);
				healthBar.SetHealth(Mathf.Min(healthBar.currentHealth+charge, 100),3f);
			}
		}

		CheckRemoveDamagingObstacle (tType.ToString());
	}

	void OnAddTool(PlayerData.ToolName tType){
		FriendTool ft = Array.Find (friendsTools, p => p.toolName == "");
		if (ft == null) {
			ft = friendsTools [0];
			Transform t = ft.friend.transform.Find (ft.toolName);
			t.gameObject.SetActive (false);
		}


		Transform tool = ft.friend.transform.Find (tType.ToString ());
		if (!tool.gameObject.activeSelf) {
			tool.gameObject.SetActive (true);
			GameObject hb = ft.friend.transform.Find ("HealthBar").gameObject;
			hb.SetActive (true);
			if (tType.ToString().Equals (PlayerData.ToolName.Restaurador.ToString ())) {							
				hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().portalEnergy;
			} else if (tType.ToString().Equals (PlayerData.ToolName.Matafuegos.ToString ())) {							
				hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().fireEnergy;
			} else if (tType.ToString().Equals (PlayerData.ToolName.Armonizador.ToString ())) {							
				hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().pollutionEnergy;
			}

			ft.friend.transform.Find ("HealthBarBackground").gameObject.SetActive (true);
			SetTool (ft, tType.ToString (), 0);
			Data.Instance.playerData.toolsNumber++;
		}
	}

	public void SetFriendTool(GameObject friend, string toolName, int tLevel){
		FriendTool ft = Array.Find (Game.Instance.toolsManager.friendsTools, p => p.friend == friend);
		SetTool (ft, toolName, tLevel);
	}

	void SetTool(FriendTool ft, string toolName, int tLevel){
		ft.toolName = toolName;
		ft.hasCharge = true;
		ft.toolLevel = tLevel;

		CheckRemoveDamagingObstacle (ft.toolName);
	}

	public void SetFriendEmpty(GameObject friend, bool remove){
		FriendTool ft = Array.Find (friendsTools, p => p.friend == friend);
		ft.hasCharge = false;

		FriendTool f = Array.Find (friendsTools, p => p.toolName == ft.toolName && p.hasCharge == true);

		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.PORTAL.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Armonizador.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.POLLUTION.ToString());

		if (remove)
			ft.toolName = "";
	}

	void CheckRemoveDamagingObstacle(string tType){
		if (tType.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && damagingObstacles.Contains(ShootObstacle.obstacleType.FIRE.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.FIRE.ToString());
		else if (tType.Equals (PlayerData.ToolName.Restaurador.ToString ()) && damagingObstacles.Contains (ShootObstacle.obstacleType.PORTAL.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.PORTAL.ToString());
		else if (tType.Equals (PlayerData.ToolName.Armonizador.ToString ()) && damagingObstacles.Contains (ShootObstacle.obstacleType.POLLUTION.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.POLLUTION.ToString());
	}

	public bool CanDamage(string tag){
		//Debug.Log ("TAG: "+tag);
		if (damagingObstacles.Count > 0) {
			return damagingObstacles.Contains (tag);
		} else {
			return false;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SelectedToolsData(){
		string result = "";
		int j = 0;
		for (int i = 0; i < friendsTools.Length; i++) {			
			if (friendsTools [i].toolName != "") {
				if (j != 0)
					result += ";";
				result += friendsTools [i].name + ":" + friendsTools [i].toolName + "_" + friendsTools [i].toolLevel;
				j++;
			}
		}
		Game.Instance.levelMetrics.tools = result;
	}

	public void FinalToolsCharge(){
		string result = "";
		int j = 0;
		for (int i = 0; i < friendsTools.Length; i++) {			
			if (friendsTools [i].toolName != "") {
				HealthBar hb = friendsTools [i].friend.transform.GetComponentInChildren<HealthBar>();
				if (j != 0)
					result += ";";
				result += friendsTools [i].name + ":" + hb.currentHealth +"%";
				j++;
			}
		}
		Game.Instance.levelMetrics.toolsEndCharge = result;
	}

	public void DisableFriend(string fn){
		FriendTool ft = Array.Find (friendsTools, p => p.name == fn);
		if (ft != null) {
			SetFriendEmpty (ft.friend, false);
			ft.friend.SetActive (false);
		}
	}

}
