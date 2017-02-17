using UnityEngine;
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
		damagingObstacles = new List<string> ();
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
			ft.toolName = tType.ToString ();
			ft.hasCharge = true;
			Data.Instance.playerData.toolsNumber++;
		}
	}

	void SetFriendEmpty(GameObject f){
		FriendTool ft = Array.Find (friendsTools, p => p.friend == f);
		ft.hasCharge = false;
		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && damagingObstacles.IndexOf (ShootObstacle.obstacleType.FIRE.ToString()) == -1)
			damagingObstacles.Add (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && damagingObstacles.IndexOf (ShootObstacle.obstacleType.PORTAL.ToString()) == -1)
			damagingObstacles.Add (ShootObstacle.obstacleType.PORTAL.ToString());
	}

	public bool CanDamage(string tag){
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
