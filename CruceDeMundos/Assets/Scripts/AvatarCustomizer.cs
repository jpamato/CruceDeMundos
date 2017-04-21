using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AvatarCustomizer : MonoBehaviour {

	public ToggleButton[] pielButton;
	public Piel[] pieles;
	public Cara[] caras;
	public Top[] torsos;
	public Bottom[] piernas;
	public Shoe[] zapatos;

	public bool isFullSizeView;

	Animator animator;

	// Use this for initialization
	void Start () {

		animator = GetComponent<Animator> ();

		SetPiel (Data.Instance.avatarData.pielIndex);
		SetCara(Data.Instance.avatarData.caraIndex);
		SetTorso(Data.Instance.avatarData.torsoIndex);
		SetPiernas(Data.Instance.avatarData.piernasIndex);
		SetZapato (Data.Instance.avatarData.zapatoIndex);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	[Serializable]
	public class Piel{
		public GameObject cabeza;
		public GameObject torax;
		public GameObject brazo1;
		public GameObject brazo1b;
		public GameObject mano1;
		public GameObject brazo2;
		public GameObject brazo2b;
		public GameObject mano2;
		public GameObject pierna1;
		public GameObject pierna1b;
		public GameObject pierna2;
		public GameObject pierna2b;

		public void SetActive(bool active){
			if(cabeza!=null)cabeza.SetActive (active);
			if(torax!=null)torax.SetActive (active);
			if(brazo1!=null)brazo1.SetActive (active);
			if(brazo1b!=null)brazo1b.SetActive (active);
			if(mano1!=null)mano1.SetActive (active);
			if(brazo2!=null)brazo2.SetActive (active);
			if(brazo2b!=null)brazo2b.SetActive (active);
			if(mano2!=null)mano2.SetActive (active);
			if(pierna1!=null)pierna1.SetActive (active);
			if(pierna1b!=null)pierna1b.SetActive (active);
			if(pierna2!=null)pierna2.SetActive (active);
			if(pierna2b!=null)pierna2b.SetActive (active);
		}
	}

	[Serializable]
	public class Cara{
		public GameObject[] ojos;
		public GameObject[] cejas;
		public GameObject[] bocas;

		int expressIndex;

		public void SetExpre(int index){
			for (int i = 0; i < ojos.Length; i++) {			
				ojos [i].SetActive (i == index);
				cejas [i].SetActive (i == index);
				bocas [i].SetActive (i == index);
			}
			expressIndex = index;
		}

		public void SetActive(bool active){
			if(ojos!=null)ojos [expressIndex].SetActive (active);
			if(cejas!=null)cejas [expressIndex].SetActive (active);
			if(bocas!=null)bocas [expressIndex].SetActive (active);
		}
	}

	[Serializable]
	public class Top{
		public GameObject torso;
		public GameObject brazoI;
		public GameObject brazoI2;
		public GameObject brazoD;
		public GameObject brazoD2;

		public void SetActive(bool active){
			if(torso!=null)torso.SetActive (active);
			if(brazoI!=null)brazoI.SetActive (active);
			if(brazoI2!=null)brazoI2.SetActive (active);
			if(brazoD!=null)brazoD.SetActive (active);
			if(brazoD2!=null)brazoD2.SetActive (active);
		}
	}

	[Serializable]
	public class Bottom{		
		public GameObject piernaI;
		public GameObject piernaI2;
		public GameObject piernaD;
		public GameObject piernaD2;

		public void SetActive(bool active){
			if(piernaI!=null)piernaI.SetActive (active);
			if(piernaI2!=null)piernaI2.SetActive (active);
			if(piernaD!=null)piernaD.SetActive (active);
			if(piernaD2!=null)piernaD2.SetActive (active);
		}
	}

	[Serializable]
	public class Shoe{		
		public GameObject shoeI;
		public GameObject shoeD;

		public void SetActive(bool active){
			if(shoeI!=null)shoeI.SetActive (active);
			if(shoeD!=null)shoeD.SetActive (active);
		}
	}

	public void SetPiel(int index){
		for (int i = 0; i < pieles.Length; i++) {			
				pieles [i].SetActive (i == index);
			if(pielButton.Length>0)pielButton [i].SetButtonOn (i == index);
		}
		Data.Instance.avatarData.pielIndex = index;
	}

	public void SetNextCara(bool next){
		if (next)
			Data.Instance.avatarData.caraIndex = Data.Instance.avatarData.caraIndex + 1 < caras.Length ? Data.Instance.avatarData.caraIndex + 1 : 0;
		else
			Data.Instance.avatarData.caraIndex = Data.Instance.avatarData.caraIndex - 1 > -1 ? Data.Instance.avatarData.caraIndex - 1 : caras.Length-1;

		SetCara (Data.Instance.avatarData.caraIndex);
	}

	public void SetCara(int index){
		for (int i = 0; i < caras.Length; i++) {			
			caras [i].SetActive (i == index);
		}
	}

	public void SetNextTorso(bool next){
		if (next)
			Data.Instance.avatarData.torsoIndex = Data.Instance.avatarData.torsoIndex + 1 < torsos.Length ? Data.Instance.avatarData.torsoIndex + 1 : 0;
		else
			Data.Instance.avatarData.torsoIndex = Data.Instance.avatarData.torsoIndex - 1 > -1 ? Data.Instance.avatarData.torsoIndex - 1 : torsos.Length-1;

		SetTorso (Data.Instance.avatarData.torsoIndex);
	}

	public void SetTorso(int index){
		for (int i = 0; i < torsos.Length; i++) {			
			torsos [i].SetActive (i == index);
		}
		if(isFullSizeView)animator.Play ("customizer_top");
	}

	public void SetNextPiernas(bool next){
		if (next)
			Data.Instance.avatarData.piernasIndex = Data.Instance.avatarData.piernasIndex + 1 < piernas.Length ? Data.Instance.avatarData.piernasIndex + 1 : 0;
		else
			Data.Instance.avatarData.piernasIndex = Data.Instance.avatarData.piernasIndex - 1 > -1 ? Data.Instance.avatarData.piernasIndex - 1 : piernas.Length-1;

		SetPiernas (Data.Instance.avatarData.piernasIndex);
	}

	public void SetPiernas(int index){
		for (int i = 0; i < piernas.Length; i++) {			
			piernas [i].SetActive (i == index);
		}
		if(isFullSizeView)animator.Play ("customizer_bottom");
	}

	public void SetNextZapato(bool next){
		if (next)
			Data.Instance.avatarData.zapatoIndex = Data.Instance.avatarData.zapatoIndex + 1 < zapatos.Length ? Data.Instance.avatarData.zapatoIndex + 1 : 0;
		else
			Data.Instance.avatarData.zapatoIndex = Data.Instance.avatarData.zapatoIndex - 1 > -1 ? Data.Instance.avatarData.zapatoIndex - 1 : zapatos.Length-1;

		SetZapato (Data.Instance.avatarData.zapatoIndex);
	}

	public void SetZapato(int index){
		for (int i = 0; i < zapatos.Length; i++) {			
			zapatos [i].SetActive (i == index);
		}
	}

}