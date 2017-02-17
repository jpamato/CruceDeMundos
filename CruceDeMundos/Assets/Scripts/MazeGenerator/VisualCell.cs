using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisualCell : MonoBehaviour 
{    
    public Transform _East;
    public Transform _West;
    public Transform _North;
    public Transform _South;

	public enum WallState{
		INVISIBLE,
		SOLID,
		FIRE,
		PORTAL,
		IN,
		OUT
	}

	public WallState eastState;
	public WallState westState;
	public WallState northState;
	public WallState southState;


	public bool isFirst;
	public bool visited=false;
	public bool enter=false;

	public VisualCell cameFrom;

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (!enter && Game.Instance.traceManager.freeTrail) {
				enter = true;

				if (Game.Instance.traceManager.lastCell != null) {
						Game.Instance.traceManager.lastCell.MakeTrail (this);
						if(!visited)cameFrom = Game.Instance.traceManager.lastCell;						
				}

				Game.Instance.traceManager.lastCell = this;
				if(!isFirst)Game.Instance.traceManager.freeTrail = false;
				//Debug.Log ("Free trail: false - " + gameObject.name + " - " + Time.frameCount);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.tag == "Player") {
			Game.Instance.traceManager.freeTrail = true;
			//Debug.Log ("Free trail: true - " + gameObject.name + " - " + Time.frameCount);
		}
	}

	public void MakeTrail(VisualCell next){
		if (enter) {			
			if (next == cameFrom) {
				gameObject.GetComponent<Renderer> ().material.color = new Color(0,0,0,0);
				visited = false;
				Events.OnNewCell ();
			} else {
				gameObject.GetComponent<Renderer> ().material.color = new Color(0,255,0,0.2f);
				visited = true;
				Events.OnNewCell ();
			}
			enter = false;
		}
	}

	public void OnEditorWallState(Transform wall, WallState state){
		//Debug.Log (wall.gameObject.name);
		if (state == WallState.INVISIBLE) {
			wall.gameObject.SetActive (false);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		} else if (state == WallState.SOLID) {
			MakeSolid (wall);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		} else if (state == WallState.FIRE) {
			MakeFire (wall);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		} else if (state == WallState.PORTAL) {
			MakePortal (wall);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		} else if (state == WallState.IN) {
			MakeIn (wall);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		} else if (state == WallState.OUT) {
			MakeOut (wall);
			Transform neighbor = GetNeighbor (wall);
			if (neighbor != null)
				neighbor.gameObject.SetActive (false);
		}
	}

	void MakeSolid(Transform wall){
		wall.gameObject.SetActive (true);
		wall.gameObject.GetComponent<Renderer> ().enabled = true;
		//wall.GetComponent<Renderer> ().material.color = Color.white;
		wall.transform.FindChild ("fire").gameObject.SetActive (false);
		wall.transform.FindChild ("portal").gameObject.SetActive (false);

	}
	void MakeFire(Transform wall){
		wall.gameObject.SetActive (true);
		wall.gameObject.GetComponent<Renderer> ().enabled = false;
		wall.transform.FindChild ("fire").gameObject.SetActive (true);
		wall.transform.FindChild ("portal").gameObject.SetActive (false);
	}
	void MakePortal(Transform wall){
		wall.gameObject.SetActive (true);
		wall.gameObject.GetComponent<Renderer> ().enabled = false;
		wall.transform.FindChild ("fire").gameObject.SetActive (false);
		wall.transform.FindChild ("portal").gameObject.SetActive (true);
	}

	void MakeIn(Transform wall){
		wall.gameObject.SetActive (true);
		wall.gameObject.GetComponent<Renderer> ().enabled = false;
	}

	void MakeOut(Transform wall){
		wall.gameObject.SetActive (true);
		wall.gameObject.GetComponent<Renderer> ().enabled = false;
	}

	Transform GetNeighbor(Transform source){
		if (source.gameObject.name.Equals ("East")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.FindChild ((int.Parse (coord [0]) + 1) + "_" + coord [1]);
			if (parent != null) {
				return parent.transform.Find ("West");
			} else {
				return null;
			}
		} else if (source.gameObject.name.Equals ("West")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.FindChild ((int.Parse (coord [0]) - 1) + "_" + coord [1]);
			if (parent != null) {
				return parent.transform.Find ("East");
			} else {
				return null;
			}
		} else if (source.gameObject.name.Equals ("North")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.FindChild (coord [0] + "_" + (int.Parse (coord [1]) - 1));
			if (parent != null) {
				return parent.transform.Find ("South");
			} else {
				return null;
			}
		} else if (source.gameObject.name.Equals ("South")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.FindChild (coord [0] + "_" + (int.Parse (coord [1]) + 1));
			if (parent != null) {
				return parent.transform.Find ("North");
			} else {
				return null;
			}
		} else {
			return null;
		}

	}

}
