using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class CharShop : MonoBehaviour {

	public GameObject friend;
	public int firstChoice;
	public GameObject[] itemButtons;
	public GameObject[] toolColumn;
		
	int lastSelected = -1;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < toolColumn.Length; i++) {
			if (i < Data.Instance.playerData.toolsNumber)
				toolColumn [i].SetActive (true);
			else
				toolColumn [i].SetActive (false);
		}
		if (firstChoice > -1) {
			Invoke ("Init", 0.1f);
		} else if (Data.Instance.playerData.toolsNumber>1) {
			firstChoice = -1 * firstChoice % 10;
			Invoke ("Init", 0.1f);
		}
	}

	void Init(){
		OnItemSelect (firstChoice);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnItemSelect(int itemN){
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.toolSelect);
		ShopItem shI = itemButtons [itemN].GetComponent<ShopItem> ();
		int lastCost = 0;

		if (lastSelected == itemN) {
			lastCost = itemButtons [lastSelected].GetComponent<ShopItem> ().val;

			Game.Instance.toolsManager.SetFriendEmpty (friend, true);

			itemButtons [itemN].GetComponent<Button> ().targetGraphic.color = Color.grey;

			Transform tool = friend.transform.Find (shI.toolName.ToString ());
			tool.gameObject.SetActive (false);
			GameObject hb = friend.transform.Find ("HealthBar").gameObject;
			hb.SetActive (false);
			friend.transform.Find ("HealthBarBackground").gameObject.SetActive (false);

			Data.Instance.playerData.resources += lastCost;
			Events.OnRefreshResources (Data.Instance.playerData.resources);

			lastSelected = -1;

		} else {

			if (lastSelected > -1) {					
				lastCost = itemButtons [lastSelected].GetComponent<ShopItem> ().val;
			}

			if (Data.Instance.playerData.resources - shI.val + lastCost >= 0) {
		
				Game.Instance.toolsManager.SetFriendEmpty (friend, true);

				foreach (GameObject item in itemButtons)
					item.GetComponent<Button> ().targetGraphic.color = Color.grey;
			
				itemButtons [itemN].GetComponent<Button> ().targetGraphic.color = Color.white;

				Transform tool = friend.transform.Find (shI.toolName.ToString ());
				if (!tool.gameObject.activeSelf) {
					//System.Enum.GetValues (PlayerData.ToolName.Matafuegos);
					string[] ttypes = System.Enum.GetNames (typeof(PlayerData.ToolName));
					for (int i = 0; i < ttypes.Length; i++) {
						if (ttypes [i].Equals (shI.toolName.ToString ())) {
							tool.gameObject.SetActive (true);
							GameObject hb = friend.transform.Find ("HealthBar").gameObject;
							hb.SetActive (true);
							if (ttypes [i].Equals (PlayerData.ToolName.Restaurador.ToString ())) {							
								hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().portalEnergy;
							} else if (ttypes [i].Equals (PlayerData.ToolName.Matafuegos.ToString ())) {							
								hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().fireEnergy;
							} else if (ttypes [i].Equals (PlayerData.ToolName.Armonizador.ToString ())) {							
								hb.GetComponent<SpriteRenderer> ().sprite = hb.GetComponent<HealthBar> ().pollutionEnergy;
							}


							friend.transform.Find ("HealthBarBackground").gameObject.SetActive (true);
						} else {
							friend.transform.Find (ttypes [i]).gameObject.SetActive (false);
						}
					}
				}
				ToolData td = tool.GetComponent<ToolData> ();
				td.CurrentLevel = td.levels [shI.level];
				Data.Instance.playerData.resources -= shI.val;

				tool.GetComponent<ShootObstacle> ().UpdateTool ();


				Data.Instance.playerData.resources += lastCost;

				Events.OnRefreshResources (Data.Instance.playerData.resources);

				/*int index = Array.IndexOf (Game.Instance.toolsManager.friends, friend);
			Game.Instance.toolsManager.toolstype [index] = shI.toolName;*/

				Game.Instance.toolsManager.SetFriendTool (friend, shI.toolName.ToString (), shI.level);

				lastSelected = itemN;
			}
		}
	}
}
