using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltearGlow : MonoBehaviour {

	public float rango;
	RectTransform rt;

	float w,h;

	// Use this for initialization
	void Start () {
		rt = GetComponent<RectTransform> ();
		w = rt.sizeDelta.x;
		h = rt.sizeDelta.y;
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Game.Instance.globalGlow * rango;
		rt.sizeDelta = new Vector2 (w+delta, h+delta);
	}
}
