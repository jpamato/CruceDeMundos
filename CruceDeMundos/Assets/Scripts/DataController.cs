using UnityEngine;
using System.Collections;

public class DataController : MonoBehaviour {

	string[] test1 = {"020121600","2","1","tarde"};
	string[] test2 = { "020088300", "2", "1", "tarde" };

	//const string URL = "http://127.0.0.1:8000/";
	//const string URL = "http://cdm.com.ar/";
	//const string URL = "http://yaguar.alwaysdata.net/";
	//const string URL = "http://10.9.5.223/";
	const string URL = "http://crucemundos.dev.gcba.gob.ar/";


	private string createUser_URL = URL + "users/create?";
	private string addLevel_URL = URL + "level/add?";
	private string addReply_URL = URL + "reply/add?";
	private string addDialog_URL = URL + "dialog/add?";
	private string addMission_URL = URL + "missions/add?";
	private string addLevelData_URL = URL + "levelData/add?";

	private string secretKey = "mondongo";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator CreateUserRoutine(string _userid, string _compid, string _username)
	{
		// username = username.Replace(" ", "_");
		string hash = Md5Test.Md5Sum(secretKey);
		//string style = Data.Instance.playerSettings.heroData.styles.style;
		if (_userid != "" && _compid != "") {
			string post_url = createUser_URL + "userid=" + WWW.EscapeURL (_userid) + "&computerid=" + WWW.EscapeURL (_compid) +
				"&username=" + WWW.EscapeURL (_username) + "&cue=" + WWW.EscapeURL (test1[0]) + "&grado=" + WWW.EscapeURL (test1[1]) + 
				"&division=" + WWW.EscapeURL (test1[2]) + "&turno=" + WWW.EscapeURL (test1[3]) + "&hash="+hash;
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

	public IEnumerator SaveLevelData(string _userid, string _compid, int level, string tools, string toolsEnd, string missions, int portalDone,
		int fireDone, int pollutionDone, int mapchecks, float levelTime, float gameTime, float mapTime, float missionTime, float toolsTime, 
		string mapTrail, string mapDeadEnds, int rtB, int rtAT, int rtE, int portalCharge, int fireCharge, int pollutionCharge, int rtCharge, int giveup)
	{
		string hash = Md5Test.Md5Sum(_userid + _compid  + secretKey);

		string post_url = addLevel_URL + "user_id=" + WWW.EscapeURL (_userid) + "&computer_id=" + WWW.EscapeURL (_compid) +
			"&level_id=" + level + "&tools_selected=" + WWW.EscapeURL (tools) + "&tools_end=" + WWW.EscapeURL (toolsEnd) + "&missions=" + WWW.EscapeURL (missions) +
				"&portal_done="+ portalDone + "&fire_done="+ fireDone + "&pollution_done="+ pollutionDone +
				"&map_checks="+ mapchecks + "&level_time=" + levelTime + "&game_time=" + gameTime +
				"&first_map_time=" + mapTime + "&mission_time=" + missionTime + "&tools_time=" + toolsTime +
			"&map_trail=" + WWW.EscapeURL (mapTrail) + "&map_dead_ends=" + WWW.EscapeURL (mapDeadEnds) + "&rt_begin=" + rtB +
			"&rt_after_tools=" + rtAT + "&rt_end=" + rtE + "&portal_charge=" + portalCharge + "&fire_charge=" + fireCharge + 
			"&pollution_charge=" + pollutionCharge + "&rt_charge=" + rtCharge + "&give_up=" + giveup + "&hash="+hash;
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
		string hash = Md5Test.Md5Sum(_userid + _compid  + secretKey);
		//string style = Data.Instance.playerSettings.heroData.styles.style;

		string post_url = addReply_URL + "user_id=" + WWW.EscapeURL (_userid) + "&computer_id=" + WWW.EscapeURL (_compid) +
            "&character_name=" + WWW.EscapeURL (character) + "&level_id=" + level + "&dialog_index=" + index +
			"&dialog_mood=" + WWW.EscapeURL (mood) + "&answer_id=" + answerId + "&hash="+hash;

		print ("addReply : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar reply: " + hs_post.error);
		else
			print ("reply agregada: " + hs_post.text);

	}

	public IEnumerator AddDialog(string character, int level, int index, string dType, string mood, string prompt, int answerId,
		string answer, string replyType, string replySubType, string indicVal)
	{

		yield return new WaitForSeconds(1f);
		string hash = Md5Test.Md5Sum(secretKey);

		string post_url = addDialog_URL + "&character_name=" + WWW.EscapeURL (character) + "&level_id=" + level +
			"&dialog_index=" + index + "&dialog_type=" + WWW.EscapeURL (dType) + "&dialog_mood=" + WWW.EscapeURL (mood) +
			"&dialog_prompt=" + WWW.EscapeURL (prompt) + "&answer_id=" + answerId + "&answer_text=" + WWW.EscapeURL(answer) +
			"&tipo_indicador=" + WWW.EscapeURL(replyType) + "&subtipo_indicador=" + WWW.EscapeURL(replySubType) + 
			"&valor_indicador=" + WWW.EscapeURL(indicVal) + "&hash="+hash;

		print ("addDialog : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar dialogo: " + hs_post.error);
		else
			print ("dialogo agregado: " + hs_post.text);

	}

	public IEnumerator AddMission(int level, string missions)
	{
		string hash = Md5Test.Md5Sum(secretKey);

		string post_url = addMission_URL + "&level_id=" + level + "&missions=" + WWW.EscapeURL (missions) + "&hash="+hash;;

		print ("addMission : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar mision: " + hs_post.error);
		else
			print ("mision agregada: " + hs_post.text);
	}

	public IEnumerator AddLevelData(int level, string missions, int portal, int fire, int pollution, int portalCharge, int fireCharge,
		int pollutionCharge, int resourcesCharge){
		string hash = Md5Test.Md5Sum(secretKey);

		string post_url = addLevelData_URL + "&level_id=" + level + "&missions=" + WWW.EscapeURL (missions) + "&portal=" + portal +
			"&fire=" + fire + "&pollution=" + pollution + "&portalCharge=" + portalCharge + "&fireCharge=" + fireCharge +
			"&pollutionCharge=" + pollutionCharge + "&resourcesCharge=" + resourcesCharge + "&hash="+hash;

		print ("addMission : " + post_url);
		WWW hs_post = new WWW (post_url);
		yield return hs_post;
		if (hs_post.error != null)
			print ("No pudo agregar mision: " + hs_post.error);
		else
			print ("mision agregada: " + hs_post.text);

	}
}
