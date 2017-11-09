using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelfieButton : MonoBehaviour {

	public GameObject fade;

	bool isFadeIn;

	bool enabled;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable(){
		enabled = true;
		Fade (0.5f);
	}

	void OnDisable(){
		enabled = false;
	}

	public void Fade(float seconds){
		GameObject f = Instantiate (fade);
		f.transform.SetParent(Data.Instance.gameObject.transform);
		Fade fadeInOut = f.GetComponent<Fade> ();

		fadeInOut.OnBeginMethod = () => {			
		};
		fadeInOut.OnLoopMethod = () => {
			float val = Mathf.Lerp (1.0f,0.95f, fadeInOut.time);
			//image.color = new Color (c.r,c.g,c.b,val * 0.4f);
			transform.localScale = new Vector3(val,val,val);
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
