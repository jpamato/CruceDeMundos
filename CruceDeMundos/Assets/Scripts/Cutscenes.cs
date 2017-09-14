using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour {

	public CSDialogManager csdialogManager;
	public GameObject cs_UI;

	// Use this for initialization
	void Start () {
		csdialogManager = GetComponent<CSDialogManager> ();
		Invoke ("LoadInitialDialog", 1f);
	}

	void LoadInitialDialog(){
		cs_UI.SetActive (true);
		csdialogManager.LoadInitialDialog ();
	}
	
	// Update is called once per frame
	/*void Update () {
		if (Input.GetKey ("n"))
			csdialogManager.LoadDialog ("Manu");
	}*/
}
