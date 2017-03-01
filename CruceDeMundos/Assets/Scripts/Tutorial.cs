using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

	public GameObject[] step;
	public float tutoVel;
	int index=-1;

	// Use this for initialization
	void Start () {		
		
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown (0))
			ShowNext ();*/
	}

	void OnEnable(){
		Invoke ("ShowNext", 0.25f);
	}

	public void ShowNext(){
		if (index + 1 < step.Length) {
			index++;
			step [index].SetActive (true);
			if (index - 1 > -1)
				step [index - 1].SetActive (false);		
		}
	}

	public void ShowPrev(){
		if (index -1 > -1) {
			index--;

			step [index].SetActive (true);
			if (index + 1 < step.Length)
				step [index + 1].SetActive (false);
			
		}
	}

	void Done(){
		step [index].SetActive (false);
		index++;
	}
}
