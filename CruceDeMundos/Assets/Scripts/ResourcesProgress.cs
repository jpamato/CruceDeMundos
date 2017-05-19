using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesProgress : MonoBehaviour {
	
	public Text label;
	public Image image;
	public Text label2;

	void Start () {		
		Events.OnRefreshResources += OnRefreshResources;        
	}
	void OnDestroy()
	{
		Events.OnRefreshResources -= OnRefreshResources;
	}
	public void OnRefreshResources(int resources)
	{		
		label.text = resources.ToString();
		if(this.isActiveAndEnabled)
			StartCoroutine(Alarm(resources<=0));
	}

	void OnEnable(){
		label.text = "" + Data.Instance.playerData.resources;
		if(label2!=null)
			label2.text = "(" + Data.Instance.playerData.resources + ")";
	}

	IEnumerator Alarm(bool empty){		 
		image.color = Color.grey;
		yield return new WaitForSeconds (0.1f);
		image.color = Color.white;
		yield return new WaitForSeconds (0.1f);
		image.color = Color.grey;
		yield return new WaitForSeconds (0.1f);
		image.color = Color.white;
		yield return new WaitForSeconds (0.1f);
		image.color = Color.grey;
		yield return new WaitForSeconds (0.1f);
		image.color = Color.white;
		if (empty) {
			yield return new WaitForSeconds (0.1f);
			image.color = Color.grey;
		}
	}
}
