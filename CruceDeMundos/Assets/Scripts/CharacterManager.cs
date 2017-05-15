using UnityEngine;
using System.Collections;

public class CharacterManager : MonoBehaviour {

	public Camera cam;
	public GameObject character;

	Animator animator;

	public void SetCharacter(GameObject ch){
		character = Instantiate (ch) as GameObject;
		character.transform.parent = gameObject.transform;
		character.transform.localPosition = Vector3.zero;
		animator = character.GetComponentInChildren<Animator> ();
		if(animator==null)
			animator = character.GetComponent<Animator> ();
		cam.enabled = true;
	}

	public void SetAnimation(string a){
		animator.Play (""+a);
	}

	public void Close(){
		cam.enabled = false;
		//character = null;
		Destroy (character.gameObject);
	}
}
