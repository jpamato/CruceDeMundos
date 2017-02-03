using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualCell : MonoBehaviour 
{    
    public Transform _East;
    public Transform _West;
    public Transform _North;
    public Transform _South;

	public bool isFirst;
	public bool visited=false;
	public bool enter=false;

	public VisualCell cameFrom;

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (!enter && Data.Instance.freeTrail) {
				enter = true;

				if (Data.Instance.lastCell != null) {
						Data.Instance.lastCell.MakeTrail (this);
						if(!visited)cameFrom = Data.Instance.lastCell;						
				}

				Data.Instance.lastCell = this;
				if(!isFirst)Data.Instance.freeTrail = false;
				//Debug.Log ("Free trail: false - " + gameObject.name + " - " + Time.frameCount);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			Data.Instance.freeTrail = true;
			//Debug.Log ("Free trail: true - " + gameObject.name + " - " + Time.frameCount);
		}
	}

	public void MakeTrail(VisualCell next){
		if (enter) {			
			if (next == cameFrom) {
				gameObject.GetComponent<Renderer> ().material.color = Color.white;
				visited = false;
			} else {
				gameObject.GetComponent<Renderer> ().material.color = Color.red;
				visited = true;
			}
			enter = false;
		}
	}
}
