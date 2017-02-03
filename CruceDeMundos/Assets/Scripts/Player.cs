using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	public float speed;
	Rigidbody rb;

	void Awake () {
		rb = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0f, moveVertical);

		//Vector3 pos = gameObject.transform.localPosition;

		//gameObject.transform.localPosition = new Vector3 (pos.x + moveHorizontal, pos.y, pos.z + moveVertical);

		//rb.AddForce (movement * speed);

		//rb.velocity = movement * speed;

		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		// Move the player to it's current position plus the movement.
		rb.MovePosition (transform.position + movement);


		GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, 0.0f);

	}
}
