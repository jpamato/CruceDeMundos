using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class Sign : MonoBehaviour {

	public Text userID;
	public Text userName;
	public Camera selfieCam;
	public RenderTexture selfieRT;
	public Button continuar;

	public Dropdown dropdown;
	string cursosJson;

	void Start () {
		StartCoroutine(GetCursos());
	}

	IEnumerator GetCursos(){
		yield return StartCoroutine(Data.Instance.dataController.GetCursos (value => cursosJson = value));

		//dropdown.options.Clear ();
		if (cursosJson != null) {
			var N = JSON.Parse (cursosJson);

			for (int i = 0; i < N.Count; i++) {
				dropdown.options.Add (new Dropdown.OptionData (){ text = "Escuela: " + N [i] ["nombre"] + " Grado " + N [i] ["grado"] + " División " + N [i] ["division"] + " Turno " + N [i] ["turno"] });
			}
		}
	}

	public void Continue(){
		selfieCam.enabled = true;
		Data.Instance.interfaceSfx.PlaySfx (Data.Instance.interfaceSfx.click1);
		Data.Instance.userId = userID.text;
		Data.Instance.userName = userName.text;
		selfieCam.enabled = false;

		Data.Instance.avatarData.CaptureSelfie (selfieRT);

		string cue = "";
		string grado = "";
		string division = "";
		string turno = "";

		if (cursosJson != null && dropdown.value>0) {
			var N = JSON.Parse (cursosJson);
			cue = N [dropdown.value-1] ["cue"];
			grado = N [dropdown.value-1] ["grado"];
			division = N [dropdown.value-1] ["division"];
			turno = N [dropdown.value-1] ["turno"];
		}

		Data.Instance.SaveUserData (cue, grado, division, turno);
		Data.Instance.avatarData.SaveAvatarData ();
		Data.Instance.LoadLevel ("LevelMap");
	}

	public void SetContinue(string s){
		if (s == "") {
			continuar.interactable = false;
		} else {
			continuar.interactable = true;
		}
	}
}
