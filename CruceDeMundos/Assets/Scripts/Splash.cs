using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Splash : MonoBehaviour {

	public LoadingBar loadingBar;

	bool loadDone;

	// Use this for initialization
	void Start () {
		if(Data.Instance.userId.Equals(""))
			StartCoroutine(AsynchronousLoad ("Sign"));
		else
			StartCoroutine(AsynchronousLoad ("LevelMap"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void NextScene(){
		loadDone = true;

	}

	IEnumerator AsynchronousLoad (string scene)
	{
		yield return null;

		AsyncOperation ao = SceneManager.LoadSceneAsync(scene);
		ao.allowSceneActivation = false;

		while (! ao.isDone)
		{			
			// [0, 0.9] > [0, 1]\
			float progress = Mathf.Clamp01(ao.progress / 0.9f);
			loadingBar.SetFill(progress);

			yield return new WaitForSeconds(1);
			// Loading completed
			/*if (ao.progress == 0.9f){
				loading.SetActive (false);
				playButton.SetActive (true);
			}*/

			if(loadDone)
				ao.allowSceneActivation = true;

			yield return null;
		}
	}
}
