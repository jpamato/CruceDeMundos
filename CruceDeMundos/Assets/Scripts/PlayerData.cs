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

}
