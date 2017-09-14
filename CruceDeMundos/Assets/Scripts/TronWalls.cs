using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TronWalls : MonoBehaviour {

	public Sprite[] areaTron;

	// Use this for initialization
	void Start () {
		SpriteRenderer sr = GetComponent<SpriteRenderer> ();
		if (Data.Instance.playerData.level < 4)
			sr.sprite = areaTron [0];
		else if(Data.Instance.playerData.level < 8)
			sr.sprite = areaTron [1];
		else
			sr.sprite = areaTron [2];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
