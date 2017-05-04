using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sign : MonoBehaviour {

	public Text userID;
	public Text userName;
	public Camera selfieCam;
	public RenderTexture selfieRT;

	public void Continue(){
		selfieCam.enabled = true;
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Data.Instance.userId = userID.text;
		Data.Instance.userName = userName.text;
		selfieCam.enabled = false;

		Data.Instance.avatarData.CaptureSelfie (selfieRT);


		Data.Instance.SaveUserData ();
		Data.Instance.avatarData.SaveAvatarData ();
		Data.Instance.LoadLevel ("LevelMap");
	}
}
