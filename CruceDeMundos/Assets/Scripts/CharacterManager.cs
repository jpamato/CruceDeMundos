using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public Camera cam;
	public GameObject character;

	Animator animator;

	public void SetCharacter(GameObject ch, bool isManu=false){
		character = Instantiate (ch) as GameObject;
		character.transform.parent = gameObject.transform;
		if (isManu)
			character.transform.localPosition = new Vector3 (0f, -4f, 0f);
		else
			character.transform.localPosition = Vector3.zero;
		animator = character.GetComponentInChildren<Animator> ();
		if(animator==null)
			animator = character.GetComponent<Animator> ();
		cam.enabled = true;
	}

	public void SetAnimation(string a){
		//Debug.Log (a);
		animator.Play (""+a);
	}

	public void Close(){
		cam.enabled = false;
		//character = null;
		Destroy (character.gameObject);
	}
}
