using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour {

	public GameObject player;
	public GameObject sprite;
	public float moveSpeed = 10f;
	public float rotationSpeed = 10f;
	public float moveDistance = 2f;



	void Start () {		
	}

	void Update () {
		float movementDistance = moveSpeed * Time.deltaTime;
		Vector3 vectorToTarget = player.transform.position - transform.position;
		if (vectorToTarget.magnitude > moveDistance ) {			
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementDistance);

			Vector3 direction = gameObject.transform.position - player.transform.position;
			direction.z = 0.0f;
			if (direction != Vector3.zero) 
				sprite.transform.rotation = Quaternion.Slerp ( sprite.transform.rotation, 
					Quaternion.FromToRotation (Vector3.left, direction), 
					rotationSpeed * Time.deltaTime);
			sprite.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,sprite.transform.rotation.eulerAngles.z));
		}


		/*Debug.Log (Mathf.Atan2 (direction.y, direction.x));
		gameObject.transform.rotation = Quaternion.AngleAxis(
			Mathf.Atan2 (direction.y, direction.x) * 360 / Mathf.PI,
			new Vector3 (0, 0, 1));*/


		
	}
}
