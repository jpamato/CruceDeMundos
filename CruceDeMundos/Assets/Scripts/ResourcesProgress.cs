using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesProgress : MonoBehaviour {

	public Text label;

	void Start () {		
		Events.OnRefreshResources += OnRefreshResources;        
	}
	void OnDestroy()
	{
		Events.OnRefreshResources -= OnRefreshResources;
	}
	public void OnRefreshResources(int resources)
	{		
		label.text = "#" + resources.ToString();
	}

	void OnEnable(){
		label.text = "#" + Data.Instance.playerData.resources;
	}
}
