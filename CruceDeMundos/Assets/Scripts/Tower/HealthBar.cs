using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public float maxHealth = 100;
	public float currentHealth = 100;
	private float originalScale;
	public Sprite portalEnergy;
	public Sprite fireEnergy;
	public Sprite pollutionEnergy;

	// Use this for initialization
	void Start () {
		originalScale = gameObject.transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector3 tmpScale = gameObject.transform.localScale;
		tmpScale.x = currentHealth / maxHealth * originalScale;
		gameObject.transform.localScale = tmpScale;
		Vector3 newPos = new Vector3 (gameObject.transform.position.x - tmpScale.x * 0.5f, gameObject.transform.position.y, gameObject.transform.position.z);
		gameObject.transform.position = newPos;*/
	}

	public float CurrentHealth {
		//2
		get {
			return currentHealth;
		}
		//3
		set {			
			currentHealth = value;
			Vector3 tmpScale = gameObject.transform.localScale;
			tmpScale.x = currentHealth / maxHealth * originalScale;
			gameObject.transform.localScale = tmpScale;
		}
	}
}
