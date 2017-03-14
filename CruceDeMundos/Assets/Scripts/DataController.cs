using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	const string URL = "http://127.0.0.1:8000/";

	private string createUser_URL = URL + "users/create?";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator CreateUserRoutine(string _userid, string _compid, string _username)
	{
		// username = username.Replace(" ", "_");
		//string hash = Md5Test.Md5Sum(_facebookID + _username  + secretKey);
		//string style = Data.Instance.playerSettings.heroData.styles.style;
		if (_userid != "" && _compid != "") {
			string post_url = createUser_URL + "userid=" + WWW.EscapeURL (_username) + "&computerid=" + WWW.EscapeURL (_compid) + "&username=" + WWW.EscapeURL (_username);
			print ("CreateUser : " + post_url);
			WWW hs_post = new WWW (post_url);
			yield return hs_post;
			if (hs_post.error != null)
				print ("No pudo crear el nuevo user: " + hs_post.error);
			else {
				print ("user agregado: " + hs_post.text);
				string json = "{";
				json += "userId:" + _userid + ",";
				json += "userName:" + _username + "}";
				PlayerPrefs.SetString ("UserData", json);
			}
		}
	}
}
