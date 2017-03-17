using UnityEngine;
using System.Collections;
using System;

public class CollectableItem : MonoBehaviour {

	public CollectableType itemType;
	public GameObject[] items;
	public int val;

	GameObject onState;
	GameObject offState;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void SetDestroy(){
		onState.SetActive (false);
		offState.SetActive (true);
		Destroy (gameObject, 2);
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (itemType == CollectableType.FIRECHARGE) {
				ToolsManager.FriendTool ft = Array.Find(Game.Instance.toolsManager.friendsTools, x => x.toolName == PlayerData.ToolName.Matafuegos.ToString());
				if (ft != null) {					
					Events.OnChargeCollect (val, PlayerData.ToolName.Matafuegos);
					SetDestroy ();
				}
			} else if (itemType == CollectableType.PORTALCHARGE) {
				ToolsManager.FriendTool ft = Array.Find(Game.Instance.toolsManager.friendsTools, x => x.toolName == PlayerData.ToolName.Restaurador.ToString());
				if (ft != null) {
					Events.OnChargeCollect (val, PlayerData.ToolName.Restaurador);
					SetDestroy ();
				}
			} else if (itemType == CollectableType.POLLUTIONCHARGE) {
				/*if (!Game.Instance.toolsManager.damagingObstacles.Contains ("POLLUTION")){
					Events.OnChargeCollect(val,PlayerData.ToolName.Armonizador);
					SetDestroy ();
				}*/
			} else if (itemType == CollectableType.RESOURCES) {
				Data.Instance.playerData.resources += val;
				Events.OnRefreshResources (Data.Instance.playerData.resources);
				SetDestroy ();
			}
		}
	}
}
