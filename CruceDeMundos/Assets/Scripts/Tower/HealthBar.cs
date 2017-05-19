using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject fade;
	public float maxHealth = 100;
	public float currentHealth = 100;
	private float originalScale;
	public Sprite portalEnergy;
	public Sprite fireEnergy;
	public Sprite pollutionEnergy;

	public SpriteRenderer border;

	//Animation anim;
	SpriteRenderer sr;

	// Use this for initialization
	void Start () {
		originalScale = gameObject.transform.localScale.x;
		//anim = GetComponent<Animation> ();
		sr = GetComponent<SpriteRenderer> ();
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


			if (value <= 0) {				
				FadeIn (border, Color.red, 1f, true);
			} else {
				border.color = Color.white;
			}
		}
	}

	public void SetHealth(float h, float rate){
		CurrentHealth = h;
		StartCoroutine (Alarm (rate));
	}

	IEnumerator Alarm(float rate){		 
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.white;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.white;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.white;
		yield return new WaitForSeconds (0.05f);
		sr.color = Color.grey;
		FadeIn (sr, Color.white, rate - 0.3f, false);
	}

	public void FadeIn(SpriteRenderer s, Color c, float seconds, bool pingpong){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeIn = f.GetComponent<Fade> ();

		fadeIn.OnLoopMethod = () => {
			float ff = Mathf.Lerp (0.25f, 1f, fadeIn.time);
			if(s!=null)
				s.color = new Color(ff*c.r,ff*c.g,ff*c.b,1.0f);
		};
		fadeIn.OnEndMethod = () => {
			if(pingpong&&currentHealth<=0)
				FadeOut(s, c, seconds,pingpong);
			fadeIn.Destroy();
		};
		fadeIn.StartFadeIn (seconds);
	}

	public void FadeOut(SpriteRenderer s, Color c, float seconds, bool pingpong){
		GameObject f = Instantiate (fade);
		f.transform.parent = Data.Instance.gameObject.transform;
		Fade fadeOut = f.GetComponent<Fade> ();

		fadeOut.OnLoopMethod = () => {
			float ff = Mathf.Lerp (0.25f, 1f, fadeOut.time);
			if(currentHealth<=0&&s!=null)
			s.color = new Color(ff*c.r,ff*c.g,ff*c.b,1.0f);
		};
		fadeOut.OnEndMethod = () => {
			if(currentHealth<=0)
				FadeIn(s, c, seconds,pingpong);
			fadeOut.Destroy();
		};
		fadeOut.StartFadeOut (seconds);
	}
}
