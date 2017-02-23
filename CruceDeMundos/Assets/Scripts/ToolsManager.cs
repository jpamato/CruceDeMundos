﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ToolsManager : MonoBehaviour {

	public FriendTool[] friendsTools;
	public List<string> damagingObstacles;

	[System.Serializable]
	public class FriendTool {
		public GameObject friend;
		public string toolName;
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
				HealthBar healthBar = friendsTools[i].friend.GetComponentInChildren<HealthBar>();
				healthBar.currentHealth = Mathf.Min(healthBar.currentHealth+charge, 100);
			}
		}
	}

	void OnAddTool(PlayerData.ToolName tType){
		FriendTool ft = Array.Find (friendsTools, p => p.toolName == "");
		Transform tool = ft.friend.transform.FindChild (tType.ToString ());
		if (!tool.gameObject.activeSelf) {
			tool.gameObject.SetActive (true);
			ft.friend.transform.FindChild ("HealthBar").gameObject.SetActive (true);
			ft.friend.transform.FindChild ("HealthBarBackground").gameObject.SetActive (true);
			SetTool (ft, tType.ToString ());
			Data.Instance.playerData.toolsNumber++;
		}
	}

	public void SetFriendTool(GameObject friend, string toolName){
		FriendTool ft = Array.Find (Game.Instance.toolsManager.friendsTools, p => p.friend == friend);
		SetTool (ft, toolName);
	}

	void SetTool(FriendTool ft, string toolName){
		ft.toolName = toolName;
		ft.hasCharge = true;
		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && damagingObstacles.Contains(ShootObstacle.obstacleType.FIRE.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && damagingObstacles.Contains (ShootObstacle.obstacleType.PORTAL.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.PORTAL.ToString());
	}

	public void SetFriendEmpty(GameObject friend){
		FriendTool ft = Array.Find (friendsTools, p => p.friend == friend);
		ft.hasCharge = false;

		FriendTool f = Array.Find (friendsTools, p => p.toolName == ft.toolName && p.hasCharge == true);

		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.PORTAL.ToString());
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

}
