using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour {

	public GameObject loading;
	public GameObject playButton;
	public float step;

	Image loadingBar;
	float fill;
	bool done;

	// Use this for initialization
	void Start () {
		loadingBar = GetComponent<Image> ();
	}

	public void SetFill(float f){
		fill = f;
	}
	
	// Update is called once per frame
	void Update () {
		if (loadingBar.fillAmount < fill) {
			loadingBar.fillAmount += step;
		} else if (!done && fill>0f) {
			loading.SetActive (false);
			playButton.SetActive (true);
			done = true;
		}
	}
}
