using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsUi : MonoBehaviour {

	public Transform avatarCol;
	public Vector2 avatarPos1;
	public Vector2 avatarPos2;

	public Transform emaCol;
	public Vector2 emaPos1;
	public Vector2 emaPos2;

	public Transform agusCol;
	public Vector2 agusPos1;
	public Vector2 agusPos2;

	// Use this for initialization
	void Start () {
		if (Data.Instance.playerData.friendsNumber < 3) {
			agusCol.GetComponent<CharShop> ().friend.SetActive (false);
			agusCol.gameObject.SetActive (false);
			avatarCol.localPosition = avatarPos1;
			emaCol.localPosition = emaPos1;
			agusCol.localPosition = agusPos1;
		} else {
			agusCol.gameObject.SetActive (true);
			avatarCol.localPosition = avatarPos2;
			emaCol.localPosition = emaPos2;
			agusCol.localPosition = agusPos2;
		}		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
