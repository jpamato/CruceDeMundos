using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleButton : MonoBehaviour {

	public GameObject buttonOn;
	public GameObject buttonOff;
	public bool defaultValue;

	// Use this for initialization
	void Start () {
		SetButtonOn (defaultValue);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetButtonOn(bool on){
		buttonOff.SetActive(!on);
		buttonOn.SetActive(on);
	}
}
