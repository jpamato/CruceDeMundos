using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CharShop : MonoBehaviour {

	public GameObject friend;
	public int firstChoice;
	public GameObject[] itemButtons;
		
	int lastSelected = -1;

	// Use this for initialization
	void Start () {
		Invoke ("Init", 0.1f);
	}

	void Init(){
		OnItemSelect (firstChoice);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnItemSelect(int itemN){
		ShopItem shI = itemButtons [itemN].GetComponent<ShopItem> ();
		int lastCost = 0;
		if (lastSelected > -1){					
			lastCost = itemButtons [lastSelected].GetComponent<ShopItem> ().val;
		}

		if(Data.Instance.playerData.resources-shI.val+lastCost>=0){
		
			foreach (GameObject item in itemButtons) 
				item.GetComponent<Button> ().targetGraphic.color = Color.green;
			
			itemButtons [itemN].GetComponent<Button> ().targetGraphic.color = Color.red;

			Transform tool = friend.transform.FindChild (shI.toolName.ToString ());
			if (!tool.gameObject.activeSelf) {
				//System.Enum.GetValues (PlayerData.ToolName.Matafuegos);
				string[] ttypes = System.Enum.GetNames(typeof(PlayerData.ToolName));
				for(int i=0;i<ttypes.Length;i++){
					if (ttypes[i].Equals (shI.toolName.ToString ()))
						tool.gameObject.SetActive (true);
					else
						friend.transform.FindChild (ttypes[i]).gameObject.SetActive (false);
					}
			}
			ToolData td = tool.GetComponent<ToolData> ();
			td.CurrentLevel = td.levels [shI.level];			

			Data.Instance.playerData.resources -= shI.val;

			Data.Instance.playerData.resources += lastCost;

			Events.OnRefreshResources (Data.Instance.playerData.resources);

			int index = Array.IndexOf (Game.Instance.toolsManager.friends, friend);
			Game.Instance.toolsManager.toolstype [index] = shI.toolName;

			lastSelected = itemN;
		}
	}
}
