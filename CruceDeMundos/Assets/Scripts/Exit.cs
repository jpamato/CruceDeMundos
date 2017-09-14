using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {

	public GameObject confirm;
	public GameObject ingame;

	// Use this for initialization
	void Start () {
		if (SceneManager.GetActiveScene ().name == "Game")
			ingame.SetActive (true);
		else
			ingame.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Confirm(){
		confirm.SetActive (true);
	}

	public void Cancel(){
		confirm.SetActive (false);
	}

	public void ConfirmExit(){
		Data.Instance.Exit ();
	}
}
