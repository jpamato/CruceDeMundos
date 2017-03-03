using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour {

	public PlayerData.ToolName toolName;
	public int level=0;

	public int val;
	Text text;

	// Use this for initialization
	void Start () {
		val = Data.Instance.playerData.tools [(int)toolName].GetComponent<ToolData> ().levels [level].cost;
		text = GetComponentInChildren<Text> ();
		text.text = val + "RT";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
