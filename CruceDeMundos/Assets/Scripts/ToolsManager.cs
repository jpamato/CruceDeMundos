using UnityEngine;
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
				healthBar.CurrentHealth = Mathf.Min(healthBar.currentHealth+charge, 100);
			}
		}
	}

	void OnAddTool(PlayerData.ToolName tType){
		FriendTool ft = Array.Find (friendsTools, p => p.toolName == "");
		Transform tool = ft.friend.transform.FindChild (tType.ToString ());
		if (!tool.gameObject.activeSelf) {
			tool.gameObject.SetActive (true);
			GameObject hb = ft.friend.transform.FindChild ("HealthBar").gameObject;
			hb.SetActive (true);
			if (tType.ToString().Equals (PlayerData.ToolName.Restaurador.ToString ())) {							
				hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().portalEnergy;
			} else if (tType.ToString().Equals (PlayerData.ToolName.Matafuegos.ToString ())) {							
				hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().fireEnergy;
			}

			ft.friend.transform.FindChild ("HealthBarBackground").gameObject.SetActive (true);
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
		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && damagingObstacles.Contains(ShootObstacle.obstacleType.FIRE.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && damagingObstacles.Contains (ShootObstacle.obstacleType.PORTAL.ToString()))
			damagingObstacles.Remove (ShootObstacle.obstacleType.PORTAL.ToString());
	}

	public void SetFriendEmpty(GameObject friend, bool remove){
		FriendTool ft = Array.Find (friendsTools, p => p.friend == friend);
		ft.hasCharge = false;

		FriendTool f = Array.Find (friendsTools, p => p.toolName == ft.toolName && p.hasCharge == true);

		if (ft.toolName.Equals (PlayerData.ToolName.Matafuegos.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.FIRE.ToString());
		else if (ft.toolName.Equals (PlayerData.ToolName.Restaurador.ToString ()) && f==null)
			damagingObstacles.Add (ShootObstacle.obstacleType.PORTAL.ToString());

		if (remove)
			ft.toolName = "";
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
		for (int i = 0; i < friendsTools.Length; i++) {
			if (i != 0)
				result += "/";
			if(friendsTools [i].toolName!="")
			result += friendsTools [i].name + "_" + friendsTools [i].toolName + "_" + friendsTools [i].toolLevel;
		}
		Game.Instance.levelMetrics.tools = result;
	}

}
