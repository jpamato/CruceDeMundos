using UnityEngine;
using System.Collections;

public class MoveEnemy : MonoBehaviour {

	public float speed=10f;
	Rigidbody2D rb;

	void Awake () {
		rb = GetComponent<Rigidbody2D> ();
		Game.Instance.traceManager.freeTrail = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Game.Instance.gameManager.state==GameManager.states.ACTIVE){
			float moveHorizontal = Input.GetAxis ("Horizontal");
			float moveVertical = Input.GetAxis ("Vertical");

			Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0f);
			RotateIntoMoveDirection (moveHorizontal, moveVertical);

			// Normalise the movement vector and make it proportional to the speed per second.
			movement = movement.normalized * speed * Time.deltaTime;
			// Move the player to it's current position plus the movement.
			rb.MovePosition (transform.position + movement);
			//rb.MovePosition(movement);

			//rb.rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);
		}

	}


	private void RotateIntoMoveDirection(float x, float y) {		
		if (x != 0f || y != 0f) {
			float rotationAngle = Mathf.Atan2 (y, x) * 180 / Mathf.PI;
			GameObject sprite = (GameObject)gameObject.transform.Find("Sprite").gameObject;
			sprite.transform.rotation = Quaternion.Euler(0f,0f,rotationAngle);
		}

	}

	public float distanceToGoal(){
		return 0f;
	}

}
