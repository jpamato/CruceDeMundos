using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootObstacle : MonoBehaviour {

	public enum obstacleType
	{
		FIRE,
		PORTAL,
		POLLUTION
	}

	public List<GameObject> obstaclesInRange;
	public obstacleType oType;
	private float lastShotTime;
	private ToolData toolData;

	private GameObject tool;

	public bool shooting;
	HealthBar healthBar;

	AudioSource source;

	// Use this for initialization
	void Start() {
		obstaclesInRange = new List<GameObject>();
		lastShotTime = Time.time;
		toolData = gameObject.GetComponentInChildren<ToolData> ();
		tool = gameObject.GetComponent<ToolData> ().CurrentLevel.visualization;

		healthBar = transform.parent.Find("HealthBar").gameObject.GetComponent<HealthBar>();

		source = GetComponent<AudioSource> ();
	}

	// Update is called once per frame
	void Update () {
		GameObject target = null;
		// 1
		float minimalObstacleDistance = float.MaxValue;
		foreach (GameObject obstacle in obstaclesInRange) {
			float distance = Vector3.Distance (obstacle.transform.position, gameObject.transform.position);
			if (distance < minimalObstacleDistance) {
				target = obstacle;
				minimalObstacleDistance = distance;
			}
		}
		// 2
		if (target != null) {
			
			if (target.gameObject.GetComponent<ObstacleData> ().CurrentLevel.health > 0) {
				if (Time.time - lastShotTime > toolData.CurrentLevel.fireRate) {
					if (healthBar.currentHealth > 0)
						shooting = true;
					else {
						shooting = false;
						if (source.isPlaying)
							source.Stop ();
					}
					Shoot (target.GetComponent<Collider2D> ());
					lastShotTime = Time.time;
				}
			} else {
				shooting = false;
				if (source.isPlaying)
					source.Stop ();
			}
			// 3
			Vector3 direction = tool.transform.position - target.transform.position;
			tool.transform.rotation = Quaternion.AngleAxis (90f +
			Mathf.Atan2 (direction.y, direction.x) * 180 / Mathf.PI,
				new Vector3 (0, 0, 1));


		} else {
			tool.transform.localRotation = Quaternion.Euler (Vector3.zero);
			shooting = false;
			if (source.isPlaying)
				source.Stop ();
		}
	}

	// 1
	void OnObstacleDestroy (GameObject obstacle) {
		obstaclesInRange.Remove (obstacle);
	}

	void OnTriggerEnter2D (Collider2D other) {
		// 2
		if (other.gameObject.tag.Equals(oType.ToString())) {
			obstaclesInRange.Add(other.gameObject);
			ObstacleDestructionDelegate del = other.gameObject.GetComponent<ObstacleDestructionDelegate>();
			del.obstacleDelegate += OnObstacleDestroy;
		}
	}
	// 3
	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag.Equals(oType.ToString())) {
			obstaclesInRange.Remove(other.gameObject);
			ObstacleDestructionDelegate del =
				other.gameObject.GetComponent<ObstacleDestructionDelegate>();
			del.obstacleDelegate -= OnObstacleDestroy;
		}
	}

	void Shoot(Collider2D target) {


		if (healthBar.currentHealth >= toolData.CurrentLevel.fireLoss) {
			source.Play ();
			shooting = true;
			GameObject bulletPrefab = toolData.CurrentLevel.bullet;
			// 1 
			Vector3 startPosition = gameObject.transform.position;
			Vector3 targetPosition = target.transform.position;
			startPosition.z = bulletPrefab.transform.position.z;
			targetPosition.z = bulletPrefab.transform.position.z;

			// 2 
			GameObject newBullet = (GameObject)Instantiate (bulletPrefab);
			newBullet.transform.position = startPosition;
			BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior> ();
			bulletComp.target = target.gameObject;
			bulletComp.startPosition = startPosition;
			bulletComp.targetPosition = targetPosition;
						
			//healthBar.CurrentHealth = Mathf.Max (healthBar.currentHealth - toolData.CurrentLevel.fireLoss, 0);
			healthBar.SetHealth(Mathf.Max (healthBar.currentHealth - toolData.CurrentLevel.fireLoss, 0),toolData.CurrentLevel.fireRate);

			if (healthBar.currentHealth == 0)
				Game.Instance.toolsManager.SetFriendEmpty (transform.parent.gameObject,false);
		}

		// 3 
		/*Animator animator = 
			toolData.CurrentLevel.visualization.GetComponent<Animator> ();
		animator.SetTrigger ("fireShot");
		AudioSource audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.PlayOneShot(audioSource.clip);*/
	}

	public void UpdateTool(){
		tool = gameObject.GetComponent<ToolData> ().CurrentLevel.visualization;
	}
}
