using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fade : MonoBehaviour
{
	public float time;
	public System.Action OnBeginMethod;
	public System.Action OnLoopMethod;
	public System.Action OnEndMethod;


	private Image masker;
	private string m_LevelName = "";
	private int m_LevelIndex = 0;
	private bool m_Fading = false;
	
	private void Awake()
	{

		DontDestroyOnLoad(this);
		masker = GetComponentInChildren<Image>();
		if (masker != null) {
			masker.enabled = true;
			masker.color = new Color (0, 0, 0, 0);
		}

		OnBeginMethod = null;
		OnLoopMethod = null;
		OnEndMethod = null;

	}
	
	private IEnumerator FadeStart(float aFadeOutTime, float aFadeInTime, Color aColor)
	{

		float t = 0;
		float fInStep = 1f / aFadeOutTime;
		float fOutStep = 1f / aFadeInTime;
		while (t < 1.0)
		{
			yield return new WaitForEndOfFrame();
			//yield return new WaitForSeconds(0.001f);
			t+=fInStep*Time.deltaTime;
			masker.color = new Color(aColor.r,aColor.g,aColor.b,t);
		}
		
		if (m_LevelName != "")
			Application.LoadLevel(m_LevelName);
		else
			Application.LoadLevel(m_LevelIndex);     
		while (t > 0f)
		{
			yield return new WaitForEndOfFrame();
			//yield return new WaitForSeconds(0.001f);
			//t-=Time.deltaTime;
			t-=fOutStep*Time.deltaTime;
			masker.color = new Color(aColor.r,aColor.g,aColor.b,t);
		}
		m_Fading = false;
	}
	private void StartFade(float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		m_Fading = true;
		StartCoroutine(FadeStart(aFadeOutTime, aFadeInTime, aColor));
	}
	
	public void LoadLevel(string aLevelName, float aFadeOutTime, float aFadeInTime, Color aColor)
	{
		//if (Fading) return;
		m_LevelName = aLevelName;
		StartFade(aFadeOutTime, aFadeInTime, aColor);
	}


	public void StartFadeIn(float aFadeTime)
	{
		m_Fading = true;
		StartCoroutine(FadeIn(aFadeTime));
	}

	private IEnumerator FadeIn(float aFadeTime)
	{
		time = 0f;
		float fInStep = 1f / aFadeTime;
		if(OnBeginMethod!=null)OnBeginMethod.Invoke();
		while (time < 1.0)
		{
			yield return new WaitForEndOfFrame();
			//yield return new WaitForSeconds(0.001f);
			time+=fInStep*Time.deltaTime;
			if(OnLoopMethod!=null)OnLoopMethod.Invoke();
		}
		if(OnEndMethod!=null)OnEndMethod.Invoke();
		m_Fading = false;
	}

	public void StartFadeOut(float aFadeTime)
	{
		m_Fading = true;
		StartCoroutine(FadeOut(aFadeTime));
	}

	private IEnumerator FadeOut(float aFadeTime)	{

		time = 1f;
		float fOutStep = 1f / aFadeTime;
		if(OnBeginMethod!=null)OnBeginMethod.Invoke();
		while (time > 0f)
		{
			yield return new WaitForEndOfFrame();
			//yield return new WaitForSeconds(0.001f);
			time-=fOutStep*Time.deltaTime;
			if(OnLoopMethod!=null)OnLoopMethod.Invoke();
		}
		if(OnEndMethod!=null)OnEndMethod.Invoke();
		m_Fading = false;
	}

	public void Destroy()
	{
		Destroy (gameObject);
	}

}
