using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFade : MonoBehaviour {

	public SpriteRenderer final,refinal;

	public GameObject fade;

	// Use this for initialization
	void Start () {
		Fade (3f);
	}

	void OnEnable(){
		Fade (3f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Fade(float seconds){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeInOut = f.GetComponent<Fade> ();
		Color fc = final.color;
		Color frc = final.color;

		fadeInOut.OnBeginMethod = () => {			
		};
		fadeInOut.OnLoopMethod = () => {
			float val = Mathf.Lerp (0, 1, fadeInOut.time);
			final.color = new Color (fc.r,fc.g,fc.b,1f-val);
			refinal.color = new Color (fc.r,fc.g,fc.b,val);
		};
		fadeInOut.OnEndMethod = () => {						
			fadeInOut.Destroy();
		};

		fadeInOut.StartFadeIn (seconds);
	}
}
