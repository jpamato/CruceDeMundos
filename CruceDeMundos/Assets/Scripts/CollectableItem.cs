﻿using UnityEngine;
using System.Collections;
using System;

public class CollectableItem : MonoBehaviour {

	public CollectableType itemType;
	public GameObject[] items;
	public int val;
	public AudioClip energyUp;
	public AudioClip rtUp;

	private AudioSource source;

	GameObject onState;
	GameObject offState;

	SpriteRenderer sr;

	public enum CollectableType{
		FIRECHARGE,
		PORTALCHARGE,
		POLLUTIONCHARGE,
		RESOURCES
	}

	// Use this for initialization
	void Start () {
		foreach (GameObject go in items)
			go.SetActive (false);

		items [(int)itemType].SetActive (true);
		onState = items [(int)itemType].transform.Find ("on").gameObject;
		offState = items [(int)itemType].transform.Find ("off").gameObject;

		sr = onState.GetComponent<SpriteRenderer> ();

		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetDestroy(){
		onState.SetActive (false);
		offState.SetActive (true);
		Destroy (gameObject, 2);
	}

	void SetBlocked(){
		sr.color = Color.red;
		StartCoroutine(Blocked());
	}

	IEnumerator Blocked(){		 
		sr.color = Color.red;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.red;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.red;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.red;
		yield return new WaitForSeconds (0.7f);
		sr.color = Color.white;
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (itemType == CollectableType.FIRECHARGE) {
				ToolsManager.FriendTool ft = Array.Find(Game.Instance.toolsManager.friendsTools, x => x.toolName == PlayerData.ToolName.Matafuegos.ToString());
				if (ft != null) {					
					Events.OnChargeCollect (val, PlayerData.ToolName.Matafuegos);
					source.PlayOneShot (energyUp);
					SetDestroy ();
					Game.Instance.levelMetrics.fireCharge++;
				} else {
					SetBlocked ();
				}
			} else if (itemType == CollectableType.PORTALCHARGE) {
				ToolsManager.FriendTool ft = Array.Find(Game.Instance.toolsManager.friendsTools, x => x.toolName == PlayerData.ToolName.Restaurador.ToString());
				if (ft != null) {
					Events.OnChargeCollect (val, PlayerData.ToolName.Restaurador);
					source.PlayOneShot (energyUp);
					SetDestroy ();
					Game.Instance.levelMetrics.portalCharge++;
				} else {
					SetBlocked ();
				}
			} else if (itemType == CollectableType.POLLUTIONCHARGE) {
				ToolsManager.FriendTool ft = Array.Find(Game.Instance.toolsManager.friendsTools, x => x.toolName == PlayerData.ToolName.Armonizador.ToString());
				if (ft != null) {
					Events.OnChargeCollect (val, PlayerData.ToolName.Armonizador);
					source.PlayOneShot (energyUp);
					SetDestroy ();
					Game.Instance.levelMetrics.pollutionCharge++;
				} else {
					SetBlocked ();
				}
			} else if (itemType == CollectableType.RESOURCES) {
				Data.Instance.playerData.resources += val;
				Events.OnRefreshResources (Data.Instance.playerData.resources);
				source.PlayOneShot (rtUp);
				Game.Instance.levelMetrics.resourcesCharge++;
				SetDestroy ();
			}
		}
	}
}
