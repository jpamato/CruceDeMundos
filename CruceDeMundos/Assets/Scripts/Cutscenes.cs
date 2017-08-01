using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscenes : MonoBehaviour {

	public CSDialogManager csdialogManager;

	// Use this for initialization
	void Start () {
		csdialogManager = GetComponent<CSDialogManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("n"))
			csdialogManager.LoadDialog ("Manu");
	}
}
