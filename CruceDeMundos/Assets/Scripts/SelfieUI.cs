using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfieUI : MonoBehaviour {

	public ToggleButton[] emojis;
	public string[] animNames;

	public Text estado;
	public Text selfieBalloon;

	public GameObject avatarSelfie;

	Animator animator;


	// Use this for initialization
	void Start () {
		animator = avatarSelfie.GetComponent<Animator> ();
		estado.text = Data.Instance.avatarData.estados [Data.Instance.avatarData.estadoIndex];
		selfieBalloon.text = estado.text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetEmoji(int index){
		for (int i = 0; i < emojis.Length; i++) {			
			emojis[i].SetButtonOn (i == index);
		}
		Debug.Log (animNames [index]);
		animator.Play (""+animNames[index]);
		//animator.Play ("contento");
	}

	public void SetEstado(bool next){
		if (next)
			Data.Instance.avatarData.estadoIndex = Data.Instance.avatarData.estadoIndex + 1 < Data.Instance.avatarData.estados.Count ? Data.Instance.avatarData.estadoIndex + 1 : 0;
		else
			Data.Instance.avatarData.estadoIndex = Data.Instance.avatarData.estadoIndex - 1 > -1 ? Data.Instance.avatarData.estadoIndex - 1 : Data.Instance.avatarData.estados.Count - 1;

		estado.text = Data.Instance.avatarData.estados [Data.Instance.avatarData.estadoIndex];
		selfieBalloon.text = estado.text;

	}
}
