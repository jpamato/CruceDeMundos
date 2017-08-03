using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsOver : MonoBehaviour {

	public GameObject info;
	public GameObject[] toolsInfo;

	// Use this for initialization
	void Start () {
		Events.OnToolButtonEnter += OnToolButtonEnter;
		Events.OnToolButtonExit += OnToolButtonExit;
	}

	void OnDestroy(){
		Events.OnToolButtonEnter -= OnToolButtonEnter;
		Events.OnToolButtonExit -= OnToolButtonExit;
	}
	
	void OnToolButtonEnter(int index){
		info.SetActive (true);
		for (int i = 0; i < toolsInfo.Length; i++)
			toolsInfo [i].SetActive (i==index);
	}

	void OnToolButtonExit(){
		info.SetActive (false);
	}
}
