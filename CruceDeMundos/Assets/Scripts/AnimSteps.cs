using UnityEngine;
using System.Collections;

public class AnimSteps : MonoBehaviour {

	public GameObject fade;
	public int step;
	private Animation anim;

	public GameObject toDestroy;
	// Use this for initialization

	private AudioSource source;

	void Start () {
		source = gameObject.GetComponent<AudioSource> ();
		anim = GetComponent<Animation> ();
		/*Debug.Log ("GO: " + gameObject.name + " anim: ");
		foreach(AnimationState aS in anim)
			Debug.Log(aS.name);*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetNextStep(int nextStep){
		step = nextStep;
		anim.Play ();
	}

	public void CheckStep(int step2Check){
		//Debug.Log (step + " - " + step2Check);	
		if (step<step2Check)
			anim.Play ();
		else
			anim.Stop();
	}

	public void DestroyGO(){
		//Debug.Log ("Destroy: " + toDestroy.name);
		toDestroy.GetComponent<CircleCollider2D> ().enabled = false;
		FadeOut (4f);
		Destroy (toDestroy, 5);
	}

	public void FadeOut(float seconds){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeOut = f.GetComponent<Fade> ();

		fadeOut.OnBeginMethod = () => {			
		};
		fadeOut.OnLoopMethod = () => {
			float vol = Mathf.Lerp (0, 1, fadeOut.time);
			if(source!=null)
				source.volume = vol;
		};
		fadeOut.OnEndMethod = () => {
			if(source!=null){
				source.Stop();
				source.volume = 1f;
			}
			fadeOut.Destroy();
		};
		fadeOut.StartFadeOut (seconds);
	}
}
