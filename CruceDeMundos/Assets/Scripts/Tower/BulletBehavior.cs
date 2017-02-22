using UnityEngine;
using System.Collections;

public class BulletBehavior : MonoBehaviour {

	public float speed = 50;
	public int damage;
	public GameObject target;
	public Vector3 startPosition;
	public Vector3 targetPosition;

	private float distance;
	private float startTime;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		distance = Vector3.Distance (startPosition, targetPosition);
	}
	
	// Update is called once per frame
	void Update () {
		// 1 
		float timeInterval = Time.time - startTime;
		gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

		// 2 
		if (gameObject.transform.position.Equals(targetPosition)) {
			if (target != null) {
				
				// 3
				/*SpriteRenderer sprite = target.GetComponent<ObstacleData>().CurrentLevel.visualization.GetComponentInChildren<SpriteRenderer>();
				float power = sprite.color.a;
				power -= Mathf.Max(damage*0.01f, 0);
				// 4
				if (power <= 0) {
					Events.OnObstacleDestroy (target.gameObject.tag);
					Destroy(target);
					//gameManager.Gold += 50;
				}
				sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,power);*/

				if(target.GetComponent<ObstacleData>().RecibeDamage(damage)){
					Events.OnObstacleDestroy (target.gameObject.tag);
					//Destroy(target);
				}
			}
			Destroy(gameObject);
		}
	}
}
