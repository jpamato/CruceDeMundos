using UnityEngine;
using System.Collections;

public class CollectableItem : MonoBehaviour {

	public CollectableType itemType;
	public int val;

	public enum CollectableType{
		FIRECHARGE,
		PORTALCHARGE,
		POLLUTIONCHARGE,
		RESOURCES
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {		
		if (other.tag == "Player") {
			if (itemType == CollectableType.FIRECHARGE) {				
				Events.OnChargeCollect(val,PlayerData.ToolName.Matafuegos);
			} else if (itemType == CollectableType.PORTALCHARGE) {				
				Events.OnChargeCollect(val,PlayerData.ToolName.Restaurador);
			} else if (itemType == CollectableType.POLLUTIONCHARGE) {				
				//Events.OnChargeCollect(val,PlayerData.ToolName.Armonizador);
			} else if (itemType == CollectableType.RESOURCES) {
				Data.Instance.playerData.resources += val;
				Events.OnRefreshResources (Data.Instance.playerData.resources);
			}
			Destroy (gameObject);
		}
	}
}
