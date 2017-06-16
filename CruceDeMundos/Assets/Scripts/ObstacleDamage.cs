using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObstacleDamage : MonoBehaviour {

	public enum toolType
	{
		FIRETOOL,
		PORTALTOOL,
		POLLUTIONTOOL
	}

	public List<GameObject> toolsInRange;
	public toolType[] tType;
	public SpriteRenderer field;
	private float lastShotTime;
	private ObstacleData obstacleData;

	// Use this for initialization
	void Start() {
		toolsInRange = new List<GameObject>();
		lastShotTime = Time.time;
		obstacleData = gameObject.GetComponentInChildren<ObstacleData> ();
	}

	// Update is called once per frame
	void Update () {
		GameObject target = null;
		// 1
		float minimalObstacleDistance = float.MaxValue;
		foreach (GameObject obstacle in toolsInRange) {
			float distance = Vector3.Distance (obstacle.transform.position, gameObject.transform.position);
			if (distance < minimalObstacleDistance) {
				target = obstacle;
				minimalObstacleDistance = distance;
				//field.color = new Color (field.color.r, field.color.g, field.color.b, 0.1f);
			}
		}
		// 2
		if (target != null) {
			if (Time.time - lastShotTime > obstacleData.CurrentLevel.fireRate) {				
				Shoot(target);
				lastShotTime = Time.time;
				//field.color = new Color (field.color.r, field.color.g, field.color.b, 1f);
			}
		} else {
			//field.color = new Color (field.color.r, field.color.g, field.color.b, 0.0f);
		}
	}

	// 1
	void OnObstacleDestroy (GameObject obstacle) {
		toolsInRange.Remove (obstacle);
	}

	void OnTriggerEnter2D (Collider2D other) {
		// 2
		foreach (toolType t in tType) {
			if (other.gameObject.tag.Equals (t.ToString ())) {			
				if (Game.Instance.toolsManager.CanDamage (gameObject.tag)) {
					toolsInRange.Add (other.gameObject);
					ToolDestructionDelegate del = other.gameObject.GetComponent<ToolDestructionDelegate> ();
					del.toolDelegate += OnObstacleDestroy;
				}
			}
		}
	}
	// 3
	void OnTriggerExit2D (Collider2D other) {
		foreach (toolType t in tType) {
			if (other.gameObject.tag.Equals (t.ToString ())) {
				toolsInRange.Remove (other.gameObject);
				ToolDestructionDelegate del =
					other.gameObject.GetComponent<ToolDestructionDelegate> ();
				del.toolDelegate -= OnObstacleDestroy;
			}
		}
	}

	void Shoot(GameObject target) {
		Transform healthBarTransform = target.transform.parent.Find("HealthBar");
		HealthBar healthBar = 
			healthBarTransform.gameObject.GetComponent<HealthBar>();
		healthBar.CurrentHealth = Mathf.Max(healthBar.currentHealth-obstacleData.CurrentLevel.damage, 0);
		// 4
		if (healthBar.currentHealth <= 0) {
			Game.Instance.toolsManager.SetFriendEmpty (target.transform.parent.gameObject, true);
			Destroy(target);
			Game.Instance.levelManager.ToolLose ();
		}
	}
}
