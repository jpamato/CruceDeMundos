using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnFonoOn : MonoBehaviour {

	public Text selfieBalloon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		if (Data.Instance.avatarData.estados.Count > 0)
			selfieBalloon.text = Data.Instance.avatarData.estados [Data.Instance.avatarData.estadoIndex];
	}
}
