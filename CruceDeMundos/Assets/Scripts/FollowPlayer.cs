using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPlayer : MovingCharacter {

	public bool isAvatar;
	public GameObject player;
	public GameObject avatar;
	public float moveSpeed = 10f;
	public float rotationSpeed = 10f;
	public float moveDistance = 2f;
	public float lostMoveDistance = 2f;
	public float lostSpeedFactor = 3f;

	private float rotationOffset = -90f;

	private Animator animator;
	MovingCharacter movePlayer;

	List<Vector3> pathList;
	public int pathCount;
	public Vector3 nextPoint;

	public bool lost;

	float lostDistance;
	//List<GameObject> points;

	void Start () {		
		animator = avatar.GetComponent<Animator> ();
		movePlayer = player.GetComponent<MovePlayer> ();
		if(movePlayer==null)
			movePlayer = player.GetComponent<FollowPlayer> ();

		//Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		//points = new List<GameObject> ();
	}

	void Update () {
		float movementDistance = moveSpeed * Time.deltaTime;
		float d = 0;
		Vector3 vectorToTarget = Vector3.zero;
		if (lost) {
			//print("count: "+pathCount);
			vectorToTarget = pathList [pathCount] - transform.position;
			//print (pathList [pathCount] + " - " + transform.position + "=" + vectorToTarget);
			d = lostMoveDistance;
		} else {
			vectorToTarget = player.transform.position - transform.position;
			d = moveDistance;
		}
		
		if (vectorToTarget.magnitude > d) {			

			Vector3 direction = Vector3.zero;
			if (lost) {
				transform.position = Vector3.MoveTowards (transform.position, pathList [pathCount], movementDistance * lostSpeedFactor);
				direction = transform.position - pathList [pathCount];
			} else {
				transform.position = Vector3.MoveTowards (transform.position, player.transform.position, movementDistance);
				direction = gameObject.transform.position - player.transform.position;
			}


			direction.z = 0.0f;
			if (direction != Vector3.zero)
				avatar.transform.rotation = Quaternion.Slerp (avatar.transform.rotation, 
					Quaternion.FromToRotation (Vector3.down, direction), 
					rotationSpeed * Time.deltaTime);
			avatar.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, avatar.transform.rotation.eulerAngles.z));
		} else if (lost) {
			pathCount++;
			if (pathCount >= pathList.Count)
				lost = false;
		}

		if (movePlayer.moving && !moving) {
			moving = true;
			animator.Play ("start");
			if(lost){
				pathCount++;
				if (pathCount >= pathList.Count)
					lost = false;
			}
		} else if (!movePlayer.moving && moving) {
			moving = false;
			animator.Play ("stop");
			if(lost){
				pathCount++;
				if (pathCount >= pathList.Count)
					lost = false;
			}
		}

		/*Debug.Log (Mathf.Atan2 (direction.y, direction.x));
		gameObject.transform.rotation = Quaternion.AngleAxis(
			Mathf.Atan2 (direction.y, direction.x) * 360 / Mathf.PI,
			new Vector3 (0, 0, 1));*/


		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "wall" && !isAvatar && !lost) {
			lostDistance = Vector3.Distance (transform.position, player.transform.position);
			StartCoroutine (CheckLostDistance());
		}
	}

	IEnumerator CheckLostDistance(){
		yield return new WaitForSeconds (0.2f);
		float d = Vector3.Distance (transform.position, player.transform.position);
		if (d > moveDistance) {
			lost = false;
			pathList = Game.Instance.pathfinder2.FindPath (transform.position, player.transform.position);
			pathCount = 0;
			yield return new WaitForSeconds (0.2f);
			if (pathList.Count > 0) {
				lost = true;
				nextPoint = pathList [pathCount];
				//points.Clear ();
				/*foreach (Vector3 p in pathList) {					
					GameObject point = GameObject.CreatePrimitive (PrimitiveType.Quad);
					point.transform.position = new Vector3 (p.x, p.y, -0.5f);
					points.Add (point);
				}*/
			}
			yield return new WaitForSeconds (4.6f);
			StartCoroutine (CheckLostDistance ());
		} else if (d < moveDistance){
			lost = false;
		}else if (lost) {
			yield return new WaitForSeconds (4);
			StartCoroutine (CheckLostDistance());
		} else {
			yield return null;
		}
	}
}
