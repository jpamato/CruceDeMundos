using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPlayer2 : MonoBehaviour {

	public GameObject player;
	public GameObject sprite;
	public float moveSpeed = 10f;
	public float rotationSpeed = 10f;
	public float moveDistance = 2f;
	public float maxMoveDistance = 10f;

	public float distance2target = 10f;

	bool lost = false;

	int nextPoint = 0;
	BoxCollider2D spriteCollider;

	void Start () {				
		Events.OnNewCell += OnNewCell;
		spriteCollider = sprite.GetComponent<BoxCollider2D> ();
	}

	void OnDestroy(){
		Events.OnNewCell -= OnNewCell;
	}


	void Update () {
		float movementDistance = moveSpeed * Time.deltaTime;
		Vector3 vectorToTarget = player.transform.position - transform.position;
		if (vectorToTarget.magnitude > moveDistance && (vectorToTarget.magnitude < maxMoveDistance || lost)) {			
			
			/*if(vectorToTarget.magnitude > maxMoveDistance )
				transform.position = Vector3.MoveTowards(transform.position, GetNextPoint(), movementDistance);
			else
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movementDistance);*/

			Vector3 direction = Vector3.zero;
			if (lost) {
				transform.position = Vector3.MoveTowards (transform.position, Game.Instance.pathfinder.lastTrace [nextPoint], movementDistance);
				direction = gameObject.transform.position - new Vector3 (Game.Instance.pathfinder.lastTrace [nextPoint].x, Game.Instance.pathfinder.lastTrace [nextPoint].y, 0f);
				direction.z = 0.0f;
				if ((Game.Instance.pathfinder.lastTrace [nextPoint] - new Vector2 (transform.position.x, transform.position.y)).sqrMagnitude < 2)
					nextPoint++;
					//Data.Instance.lastPointIndex = nextPoint;
				if (nextPoint > Game.Instance.pathfinder.lastTrace.Count - 1) {
					nextPoint = 0;
					//Data.Instance.lastPointIndex = nextPoint;
					lost = false;
					Debug.Log ("NO Lost");
					Game.Instance.pathfinder.TraceReset ();
				}

			} else {
				transform.position = Vector3.MoveTowards (transform.position, player.transform.position, movementDistance);
				direction = gameObject.transform.position - player.transform.position;
				direction.z = 0.0f;
			}

			if (direction != Vector3.zero)
				sprite.transform.rotation = Quaternion.Slerp (sprite.transform.rotation, 
					Quaternion.FromToRotation (Vector3.left, direction), 
					rotationSpeed * Time.deltaTime);
			sprite.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, sprite.transform.rotation.eulerAngles.z));

		} else if (vectorToTarget.magnitude > maxMoveDistance && !lost) {			
			lost = true;
			Debug.Log ("Lost");
		}else if(vectorToTarget.magnitude <= moveDistance && lost){
			lost = false;
			Debug.Log ("NO Lost");
			Game.Instance.pathfinder.TraceReset ();
			nextPoint = 0;
		}

		/*Debug.Log (Mathf.Atan2 (direction.y, direction.x));
		gameObject.transform.rotation = Quaternion.AngleAxis(
			Mathf.Atan2 (direction.y, direction.x) * 360 / Mathf.PI,
			new Vector3 (0, 0, 1));*/


		
	}

	void OnNewCell(){
		if (!lost) {
			Game.Instance.pathfinder.AddTrace (GetClosest2D (new Vector2 (player.transform.position.x, player.transform.position.y), Game.Instance.pathfinder.path));
		} else {
			Game.Instance.pathfinder.lastTrace.Add (GetClosest2D (new Vector2 (player.transform.position.x, player.transform.position.y), Game.Instance.pathfinder.path));
		}
	}

	Vector3 GetNextPoint(){		
		List<Vector2> list = Game.Instance.pathfinder.path.FindAll (x => Vector2.Distance(x,new Vector2(player.transform.position.x,player.transform.position.y))<distance2target);
		//List<Vector2> list = Data.Instance.path.FindAll (x => IsPointInsideRadius(x,new Vector2(transform.position.x,transform.position.x),new Vector2(player.transform.position.x,player.transform.position.x)));
		Vector2 r = GetClosest2D (new Vector2 (transform.position.x, transform.position.y), list);
		return new Vector3 (r.x, r.y, 0f);
	}

	Vector2 GetClosest2D (Vector2 pos, List<Vector2> list){
				Vector2 bestTarget = Vector2.zero;
				float closestDistanceSqr = Mathf.Infinity;
				foreach(Vector2 potentialTarget in list)
				{
					Vector2 directionToTarget = potentialTarget - pos;
					float dSqrToTarget = directionToTarget.sqrMagnitude;
					if(dSqrToTarget < closestDistanceSqr)
					{
						closestDistanceSqr = dSqrToTarget;
						bestTarget = potentialTarget;
					}
				}

				return bestTarget;
	}

	bool IsPointInsideRadius(Vector2 point, Vector2 source, Vector2 target){
		Vector2 point2TargetDir = point - target;
		float distPoint2Target = point2TargetDir.sqrMagnitude;

		Vector2 sourcet2TargetDir = source - target;
		float distSource2Target = sourcet2TargetDir.sqrMagnitude;
		return distPoint2Target < distSource2Target * 0.9f;
	}

	void OnCollisionEnter (Collision col)
	{
		//Debug.Log (col.gameObject.name);
		Debug.Log ("ACA");

	}

	/*void OnTriggerEnter2D (Collider2D other) {		
		if (!lost && (other.name == "East" || other.name == "West" || other.name == "North" || other.name == "South")) {
			//lost = true;
			if(other.gameObject.activeSelf)
				Debug.Log (other.name+" - "+other.transform.parent.gameObject.name);
			//Debug.Log("lost: "+lost);

		}
	}*/

	/*void OnTriggerExit2D (Collider2D other) {
		lost = false;
		Debug.Log("lost: "+lost);
	}*/
}
