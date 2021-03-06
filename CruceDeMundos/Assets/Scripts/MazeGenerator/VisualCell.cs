﻿using UnityEngine;
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
		OBSTACLE,
		FIRE,
		PORTAL,
		IN,
		OUT
	}

	public WallState eastState;
	public WallState westState;
	public WallState northState;
	public WallState southState;

	public bool deadEnd;
	public int visitTimes;

	public bool isFirst;
	public bool visited=false;
	public bool enter=false;

	public VisualCell cameFrom;

	Renderer renderer;

	void Start(){
		renderer = gameObject.GetComponent<Renderer> ();
	}

	void Update(){
		if (visited)
			renderer.material.color = new Color (0, 0.6f, 0, Game.Instance.globalGlow*0.6f);
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (!enter && Game.Instance.traceManager.freeTrail) {
				enter = true;

				if (Game.Instance.traceManager.lastCell != null) {
						Game.Instance.traceManager.lastCell.MakeTrail (this);
					if (!visited)
						cameFrom = Game.Instance.traceManager.lastCell;
					else
						Events.OnFalseTrail (this,this);
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
				SetVisited (false);
				Events.OnNewCell ();
			} else {
				SetVisited (true);
				Events.OnNewCell ();
			}
			enter = false;
		}
	}

	public void SetVisited(bool v){
		visited = v;
		if (v) {
			renderer.material.color = new Color (0, 0.6f, 0, 0.6f);
			if (deadEnd)
				visitTimes++;
		} else {
			renderer.material.color = new Color (0, 0, 0, 0);
			//Events.OnFalseTrail (this,this);
		}
	}

	public void OnEditorWallState(Transform wall, WallState state){
		//Debug.Log (wall.gameObject.name);
		if (state == WallState.INVISIBLE) {
			wall.gameObject.SetActive (false);
			MuteNeighbor (wall);
		} else if (state == WallState.SOLID) {
			MakeSolid (wall);
			MuteNeighbor (wall);
		} else if (state == WallState.FIRE) {
			MakeFire (wall);
			MuteNeighbor (wall);
		} else if (state == WallState.PORTAL) {
			MakePortal (wall);
			MuteNeighbor (wall);
		} else if (state == WallState.IN) {
			MakeIn (wall);
			MuteNeighbor (wall);
		} else if (state == WallState.OUT) {
			MakeOut (wall);
			MuteNeighbor (wall);
		} else if (state == WallState.OBSTACLE) {
			MakeObstacle (wall);
			MuteNeighbor (wall);
		}
	}

	void MakeSolid(Transform wall){
		wall.gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = true;
		//wall.GetComponent<Renderer> ().material.color = Color.white;
		wall.transform.Find ("tronWall").gameObject.SetActive (true);
		wall.transform.Find ("fire").gameObject.SetActive (false);
		wall.transform.Find ("portal").gameObject.SetActive (false);

	}
	void MakeFire(Transform wall){
		wall.gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = false;
		wall.transform.Find ("fire").gameObject.SetActive (true);
		wall.transform.Find ("portal").gameObject.SetActive (false);
	}
	void MakePortal(Transform wall){
		wall.gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = false;
		wall.transform.Find ("fire").gameObject.SetActive (false);
		wall.transform.Find ("portal").gameObject.SetActive (true);
	}

	void MakeIn(Transform wall){
		wall.gameObject.SetActive (true);
		GameObject exit = wall.parent.Find ("exit").gameObject;
		exit.SetActive (true);
		exit.transform.Find (wall.name).gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = false;
	}

	void MakeOut(Transform wall){
		wall.gameObject.SetActive (true);
		GameObject exit = wall.parent.Find ("exit").gameObject;
		exit.SetActive (true);
		exit.transform.Find (wall.name).gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = false;
	}

	void MakeObstacle(Transform wall){
		wall.gameObject.SetActive (true);
		//wall.gameObject.GetComponent<Renderer> ().enabled = true;
		//wall.GetComponent<Renderer> ().material.color = Color.white;
		wall.transform.Find ("tronWall").gameObject.SetActive (false);
	}

	void MuteNeighbor(Transform source){
		if (source.gameObject.name.Equals ("East")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.Find ((int.Parse (coord [0]) + 1) + "_" + coord [1]);
			if (parent != null) {
				parent.transform.Find ("West").gameObject.SetActive(false);
				parent.GetComponent<VisualCell> ().westState = WallState.INVISIBLE;
			}			
		} else if (source.gameObject.name.Equals ("West")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.Find ((int.Parse (coord [0]) - 1) + "_" + coord [1]);
			if (parent != null) {
				parent.transform.Find ("East").gameObject.SetActive(false);
				parent.GetComponent<VisualCell> ().eastState = WallState.INVISIBLE;
			}
		} else if (source.gameObject.name.Equals ("North")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.Find (coord [0] + "_" + (int.Parse (coord [1]) - 1));
			if (parent != null) {
				parent.transform.Find ("South").gameObject.SetActive(false);
				parent.GetComponent<VisualCell> ().southState = WallState.INVISIBLE;
			}
		} else if (source.gameObject.name.Equals ("South")) {
			string[] coord = source.transform.parent.name.Split ('_');
			Transform parent = source.transform.parent.transform.parent.Find (coord [0] + "_" + (int.Parse (coord [1]) + 1));
			if (parent != null) {
				parent.transform.Find ("North").gameObject.SetActive(false);
				parent.GetComponent<VisualCell> ().northState = WallState.INVISIBLE;
			}
		}
	}


	public void SetObstacle(Transform wall){
		wall.Find ("tronWall").gameObject.SetActive(false);
	}
}
