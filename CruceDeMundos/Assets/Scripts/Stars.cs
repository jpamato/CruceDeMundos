using UnityEngine;
using System.Collections;

public class Stars : MonoBehaviour {

	public GameObject[] stars;

	public void SetStarOn(int i){
		stars [i].transform.Find ("on").gameObject.SetActive (true);
	}
}
