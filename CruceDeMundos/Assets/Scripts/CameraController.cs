using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	public GameObject player;       //Public variable to store a reference to the player game object

	private Vector3 offset;         //Private variable to store the offset distance between the player and camera
	private Camera cam;

	public bool zoom = true;
	public Zoom zoomIntro;
	public Zoom zoomIn;
	public Zoom zoomOut;

	[Serializable]
	public class Zoom
	{
		public float camSize;
		public Vector3 camPos;
	}

	// Use this for initialization
	void Start () 
	{
		//Calculate and store the offset value by getting the distance between the player's position and camera's position.
		cam = gameObject.GetComponent<Camera> ();
		CamZoom(true);
		offset = transform.position - player.transform.position;
		GameIntro ();
		Events.GameActive += GameActive;
		Events.GameMap += GameMap;
		//Events.GameIntro += GameIntro;
	}

	void OnDestroy(){		
		Events.GameActive -= GameActive;
		Events.GameMap -= GameMap;
	}

	// LateUpdate is called after Update each frame
	void LateUpdate () 
	{
		// Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
		if(zoom)
		transform.position = player.transform.position + offset;
	}

	public void ToogleZoom(){
		zoom = !zoom;
		CamZoom (zoom);
	}

	public void CamZoom(bool z){
		zoom = z;
		if (zoom) {
			/*gameObject.transform.position = new Vector3 (0, 0, -1);
			cam.orthographicSize = 12;*/
			gameObject.transform.position = zoomIn.camPos;
			cam.orthographicSize = zoomIn.camSize;

		} else {
			/*gameObject.transform.position = new Vector3 (55, -19, -1);
			cam.orthographicSize = 48;*/
			gameObject.transform.position = zoomOut.camPos;
			cam.orthographicSize = zoomOut.camSize;
		}
	}

	void GameActive(){
		CamZoom (true);
	}

	void GameMap(){
		CamZoom (false);
	}

	void GameIntro(){
		Debug.Log ("intro");
		zoom = false;
		gameObject.transform.position = zoomIntro.camPos;
		cam.orthographicSize = zoomIntro.camSize;
	}

}
