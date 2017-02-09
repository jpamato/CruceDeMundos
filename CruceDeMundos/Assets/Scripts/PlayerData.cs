using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour {

	public int level;
	public int resources;
	public int toolsNumber;

	public enum ToolName{
		Matafuegos,
		Restaurador
	}

	public GameObject[] tools;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
