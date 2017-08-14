using UnityEngine;
using System.Collections;

public class FollowPlayer : MovingCharacter {

	public GameObject player;
	public GameObject avatar;
	public float moveSpeed = 10f;
	public float rotationSpeed = 10f;
	public float moveDistance = 2f;

	private float rotationOffset = -90f;

	private Animator animator;
	MovingCharacter movePlayer;

	void Start () {		
		animator = avatar.GetComponent<Animator> ();
		movePlayer = player.GetComponent<MovePlayer> ();
		if(movePlayer==null)
			movePlayer = player.GetComponent<FollowPlayer> ();
	}

	void Update () {
		float movementDistance = moveSpeed * Time.deltaTime;
		Vector3 vectorToTarget = player.transform.position - transform.position;
		if (vectorToTarget.magnitude > moveDistance ) {			

			transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementDistance);

			Vector3 direction = gameObject.transform.position - player.transform.position;
			direction.z = 0.0f;
			if (direction != Vector3.zero) 
				avatar.transform.rotation = Quaternion.Slerp ( avatar.transform.rotation, 
					Quaternion.FromToRotation (Vector3.down, direction), 
					rotationSpeed * Time.deltaTime);
			avatar.transform.rotation = Quaternion.Euler (new Vector3(0f,0f, avatar.transform.rotation.eulerAngles.z));
		}

		if (movePlayer.moving && !moving) {
			moving = true;
			animator.Play ("start");
		} else if (!movePlayer.moving && moving) {
			moving = false;
			animator.Play ("stop");
		}			

		/*Debug.Log (Mathf.Atan2 (direction.y, direction.x));
		gameObject.transform.rotation = Quaternion.AngleAxis(
			Mathf.Atan2 (direction.y, direction.x) * 360 / Mathf.PI,
			new Vector3 (0, 0, 1));*/


		
	}
}
