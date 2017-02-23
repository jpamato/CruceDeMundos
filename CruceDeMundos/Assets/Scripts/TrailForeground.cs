using UnityEngine;
using System.Collections;

public class TrailForeground : MonoBehaviour {

	// Use this for initialization
 public TrailRenderer trail;
 // Use this for initialization
 void Start () {
     trail.sortingLayerName = "Default";
     trail.sortingOrder = 2;
 
 }
	
	// Update is called once per frame
	void Update () {
	
	}
}
