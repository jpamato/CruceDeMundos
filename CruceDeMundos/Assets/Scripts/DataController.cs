using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	const string URL = "http://127.0.0.1:8000/";

	private string createUser_URL = URL + "users/create?";
	private string addLevel_URL = URL + "level/add?";
	private string addReply_URL = URL + "reply/add?";

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
			string post_url = createUser_URL + "userid=" + WWW.EscapeURL (_userid) + "&computerid=" + WWW.EscapeURL (_compid) + "&username=" + WWW.EscapeURL (_username);
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

	public IEnumerator SaveLevelData(string _userid, string _compid, int level, string tools, string misions, int mapchecks,
		float levelTime, float gameTime, float mapTime, float misionTime, float toolsTime, string mapTrail, int rtB, int rtAT, int rtE)
	{
		// username = username.Replace(" ", "_");
		//string hash = Md5Test.Md5Sum(_facebookID + _username  + secretKey);
		//string style = Data.Instance.playerSettings.heroData.styles.style;

		string post_url = addLevel_URL + "user_id=" + WWW.EscapeURL (_userid) + "&computer_id=" + WWW.EscapeURL (_compid) +
				"&level_id=" + level + "&tools_selected=" + WWW.EscapeURL (tools) + "&misions=" + WWW.EscapeURL (misions) +
				"&map_checks="+ mapchecks + "&level_time=" + levelTime + "&game_time=" + gameTime +
				"&first_map_time=" + mapTime + "&mision_time=" + misionTime + "&tools_time=" + toolsTime +
			"&map_trail=" + WWW.EscapeURL (mapTrail) + "&rt_begin=" + rtB + "&rt_after_tools=" + rtAT + "&rt_end=" + rtE;
		print ("addLevel : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar datos de nivel: " + hs_post.error);
		else
			print ("datos de nivel agregados: " + hs_post.text);
			
	}

	public IEnumerator SaveDialogData(string _userid, string _compid, string character, int level, int index, string mood, int answerId)
	{
		// username = username.Replace(" ", "_");
		//string hash = Md5Test.Md5Sum(_facebookID + _username  + secretKey);
		//string style = Data.Instance.playerSettings.heroData.styles.style;

		string post_url = addReply_URL + "user_id=" + WWW.EscapeURL (_userid) + "&computer_id=" + WWW.EscapeURL (_compid) +
            "&character_name=" + WWW.EscapeURL (character) + "&level_id=" + level + "&dialog_index=" + index +
			"&dialog_mood=" + WWW.EscapeURL (mood) + "&answer_id=" + answerId;

		print ("addReply : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar reply: " + hs_post.error);
		else
			print ("reply agregada: " + hs_post.text);

	}
}
