using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarm : MonoBehaviour {

	Image image;
	public GameObject fade;

	bool isFadeIn;

	bool enabled;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		image = GetComponent<Image> ();
		enabled = true;
		Fade (1.0f);
	}

	void OnDisable(){
		enabled = false;
	}

	public void Fade(float seconds){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeInOut = f.GetComponent<Fade> ();
		Color c = image.color;

		fadeInOut.OnBeginMethod = () => {			
		};
		fadeInOut.OnLoopMethod = () => {
			float val = Mathf.Lerp (0, 1, fadeInOut.time);
			image.color = new Color (c.r,c.g,c.b,val * 0.4f);
		};
		fadeInOut.OnEndMethod = () => {			
			isFadeIn=!isFadeIn;
			if(enabled)Fade(seconds);
			fadeInOut.Destroy();
		};
		if(isFadeIn)
			fadeInOut.StartFadeOut (seconds);
		else
			fadeInOut.StartFadeIn (seconds);
	}

}
