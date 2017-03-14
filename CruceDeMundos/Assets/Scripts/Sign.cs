using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sign : MonoBehaviour {

	public Text userID;
	public Text userName;

	public void Continue(){
		Data.Instance.userId = userID.text;
		Data.Instance.userName = userName.text;
		Data.Instance.SaveUserData ();
		Data.Instance.LoadLevel ("LevelMap");
	}
}
