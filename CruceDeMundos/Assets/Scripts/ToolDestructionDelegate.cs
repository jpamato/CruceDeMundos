using UnityEngine;
using System.Collections;

public class ToolDestructionDelegate : MonoBehaviour {

	public delegate void ToolDelegate (GameObject tool);
	public ToolDelegate toolDelegate;

	// Use this for initialization
	void Start () {

	}

	void OnDestroy () {
		if (toolDelegate != null) {
			toolDelegate (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
