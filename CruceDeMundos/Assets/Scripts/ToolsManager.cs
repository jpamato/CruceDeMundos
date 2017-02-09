using UnityEngine;
using System.Collections;
using System;

public class ToolsManager : MonoBehaviour {

	public GameObject[] friends;
	public PlayerData.ToolName[] toolstype;

	// Use this for initialization
	void Start () {		
		Events.OnChargeCollect += OnChargeCollect;		
	}

	void OnDestroy(){		
		Events.OnChargeCollect -= OnChargeCollect;
	}

	void OnChargeCollect(int val, PlayerData.ToolName tType){
		int cant = Array.FindAll (toolstype, p => p == tType).Length;
		float charge = 1f * val / cant;
		for (int i = 0; i < toolstype.Length; i++) {
			if (toolstype [i] == tType) {
				HealthBar healthBar = friends[i].GetComponentInChildren<HealthBar>();
				healthBar.currentHealth = Mathf.Min(healthBar.currentHealth+charge, 100);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
