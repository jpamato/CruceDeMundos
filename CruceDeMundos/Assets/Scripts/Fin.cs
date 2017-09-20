using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fin : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("CloseFin", 3f);
	}

	void CloseFin(){
		Data.Instance.LoadLevel ("LevelMap", 3f, 1f, Color.black);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
