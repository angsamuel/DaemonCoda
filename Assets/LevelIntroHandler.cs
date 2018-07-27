using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroHandler : MonoBehaviour {
	public Image fadeInMask;
	public Text villageNameText;
	public LevelGenerator lg;

	public float textFadeInTime = 2.0f;
	public float pauseTime = 1.0f;
	public float pauseTime2 = 1.0f;
	public float fadeOutTime = 1.0f;
	// Use this for initialization

	IEnumerator Intro(){
		villageNameText.color = new Color(1,1,1,0);
		fadeInMask.color = new Color(0,0,0,1);
		yield return new WaitForSeconds(0.5f);
		villageNameText.color = new Color(1,1,1,0);
		villageNameText.text = lg.villageName;
		float t = 0.0f;
		while(t<textFadeInTime){
			t += Time.deltaTime;
			villageNameText.color = new Color(1,1,1,t/textFadeInTime);
			yield return null;
		}
		t = 0.0f;
		while(t<pauseTime){
			t+= Time.deltaTime;
			yield return null;
		}
		villageNameText.color = new Color(1,1,1,0);
		t = 0.0f;
		while(t<pauseTime2){
			t+= Time.deltaTime;
			yield return null;
		}

		t = 0.0f;
		while(t<fadeOutTime){
			t += Time.deltaTime;
			
			fadeInMask.color = new Color(0,0,0, 1 - (t/fadeOutTime)); 
			
			yield return null;
		}
		yield return null;
	}
	void Start () {
		
		StartCoroutine(Intro());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
