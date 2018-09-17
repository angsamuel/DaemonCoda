using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelIntroOutroHandler : MonoBehaviour {
	public bool tutorial = false;
	public bool capital = false;
	public Image fadeInMask;
	public Text villageNameText;
	public LevelGenerator lg;
	public PlayerInputController pic;

	public float textFadeInTime = 2.0f;
	public float pauseTime = 1.0f;
	public float pauseTime2 = 1.0f;
	public float fadeOutTime = 1.0f;
	// Use this for initialization

	IEnumerator Intro(){
		pic.enabeled = false;
		villageNameText.color = new Color(1,1,1,0);
		fadeInMask.color = new Color(0,0,0,1);
		yield return new WaitForSeconds(0.5f);
		villageNameText.color = new Color(1,1,1,0);
		if(tutorial){
			villageNameText.text = "Tutorial";
		}else if(capital){
			villageNameText.text = "The Capital";
		}else{
			villageNameText.text = lg.villageName;
		}
		float t = 0.0f;
		while(t<textFadeInTime){
			t += Time.deltaTime;
			villageNameText.color = new Color(1,1,1,t/textFadeInTime);
			yield return null;
		}
		if(!tutorial){
			pic.transform.Translate(new Vector2(-1000,-1000));
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
		pic.enabeled = true;
		if(!tutorial){
			pic.transform.Translate(new Vector2(1000,1000));
		}
		t = 0.0f;
		while(t<fadeOutTime){
			t += Time.deltaTime;
			
			fadeInMask.color = new Color(0,0,0, 1 - (t/fadeOutTime)); 
			
			yield return null;
		}
		fadeInMask.color = new Color(0,0,0, 0); 
		fadeInMask.gameObject.SetActive(false);
		yield return null;
	}

	IEnumerator Outro(){
		fadeInMask.gameObject.SetActive(true);
		fadeInMask.color = new Color(0,0,0,0);
		float t = 0.0f;
		while(t<fadeOutTime){
			t += Time.deltaTime;
			
			fadeInMask.color = new Color(0,0,0,(t/fadeOutTime)); 
			
			yield return null;
		}
		fadeInMask.color = new Color(0,0,0,1); 
		pic.playerUnit.transform.position = new Vector3(10000,10000,10000);
		yield return new WaitForSeconds(.5f);
		//generate new menues in selection
		if(tutorial){
			SceneManager.LoadScene("MainMenu");
		}else{

			PlayerPrefs.SetString(PlayerPrefs.GetString("profile") + "settlements saved", ""); 
			SceneManager.LoadScene("LevelSelect");
		}
		//change scene
	}

	public void LeaveLevel(){
		StartCoroutine(Outro());
	}

	void Start () {
		StartCoroutine(Intro());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
