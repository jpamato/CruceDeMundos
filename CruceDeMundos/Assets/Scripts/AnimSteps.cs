using UnityEngine;
using System.Collections;

public class AnimSteps : MonoBehaviour {

	public int step;
	private Animation anim;

	public GameObject toDestroy;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation> ();
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
		if (step<=step2Check)
			anim.Play ();
		else
			anim.Stop();
	}

	public void DestroyGO(){
		toDestroy.GetComponent<CircleCollider2D> ().enabled = false;
		Destroy (toDestroy, 5);
	}
}
