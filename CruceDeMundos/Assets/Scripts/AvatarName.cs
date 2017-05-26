using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarName : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Text text = GetComponent<Text> ();
		if (text != null)
			text.text = Data.Instance.userName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
