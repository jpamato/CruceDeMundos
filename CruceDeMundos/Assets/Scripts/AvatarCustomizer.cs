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

	private int caraIndex;
	private int torsoIndex;
	private int piernasIndex;
	private int zapatoIndex;

	// Use this for initialization
	void Start () {
		/*SetCara (true);
		SetTorso (true);
		SetPiernas (true);
		SetZapato (true);*/
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
			cabeza.SetActive (active);
			torax.SetActive (active);
			brazo1.SetActive (active);
			brazo1b.SetActive (active);
			mano1.SetActive (active);
			brazo2.SetActive (active);
			brazo2b.SetActive (active);
			mano2.SetActive (active);
			pierna1.SetActive (active);
			pierna1b.SetActive (active);
			pierna2.SetActive (active);
			pierna2b.SetActive (active);
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
			ojos [expressIndex].SetActive (active);
			cejas [expressIndex].SetActive (active);
			bocas [expressIndex].SetActive (active);
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
				pielButton [i].SetButtonOn (i == index);
		}
	}

	public void SetCara(bool next){
		if (next)
			caraIndex = caraIndex + 1 < caras.Length ? caraIndex + 1 : 0;
		else
			caraIndex = caraIndex - 1 > -1 ? caraIndex - 1 : caras.Length-1;

		for (int i = 0; i < caras.Length; i++) {			
			caras [i].SetActive (i == caraIndex);
		}
	}

	public void SetTorso(bool next){
		if (next)
			torsoIndex = torsoIndex + 1 < torsos.Length ? torsoIndex + 1 : 0;
		else
			torsoIndex = torsoIndex - 1 > -1 ? torsoIndex - 1 : torsos.Length-1;

		for (int i = 0; i < torsos.Length; i++) {			
			torsos [i].SetActive (i == torsoIndex);
		}
	}

	public void SetPiernas(bool next){
		if (next)
			piernasIndex = piernasIndex + 1 < piernas.Length ? piernasIndex + 1 : 0;
		else
			piernasIndex = piernasIndex - 1 > -1 ? piernasIndex - 1 : piernas.Length-1;

		for (int i = 0; i < piernas.Length; i++) {			
			piernas [i].SetActive (i == piernasIndex);
		}
	}

	public void SetZapato(bool next){
		if (next)
			zapatoIndex = zapatoIndex + 1 < zapatos.Length ? zapatoIndex + 1 : 0;
		else
			zapatoIndex = zapatoIndex - 1 > -1 ? zapatoIndex - 1 : zapatos.Length-1;

		for (int i = 0; i < zapatos.Length; i++) {			
			zapatos [i].SetActive (i == zapatoIndex);
		}
	}

}
