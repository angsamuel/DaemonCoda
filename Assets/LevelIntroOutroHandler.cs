using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelIntroOutroHandler : MonoBehaviour {
	LevelSettings ls;
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
		if(pic!=null){
			pic.enabeled = false;
		}
		villageNameText.color = new Color(1,1,1,0);
		fadeInMask.color = new Color(0,0,0,1);
		yield return new WaitForSeconds(0.5f);
		villageNameText.color = new Color(1,1,1,0);
		if(ls.tutorial){
			villageNameText.text = "Tutorial";
		}else if(ls.capital){
			villageNameText.text = "The Capital";
		}else if(ls.loadFromPrefs && lg!=null){
			villageNameText.text = lg.villageName;
		}else if(ls.menu){
			villageNameText.text = "";
		}

		if(!ls.tutorial && pic!=null){
			pic.transform.Translate(new Vector2(-1000,-1000));
		}

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
		if(pic!=null){
			pic.enabeled = true;
		}
		if(!ls.tutorial && !ls.menu){
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
		if(ls.tutorial){
			SceneManager.LoadScene("MainMenu");
		}else{
			
			PlayerPrefs.SetString(PlayerPrefs.GetString("profile") + "settlements saved", ""); 
			SceneManager.LoadScene("LevelSelect");
		}
		//change scene
	}

	public void LeaveLevel(){
		StartCoroutine(Outro());
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "medPaks", pic.playerUnit.GetMedPaks());
		PlayerPrefs.SetInt(PlayerPrefs.GetString("profile") + "mealPaks", pic.playerUnit.GetMealPaks());
	}

	void Start () {
		ls = GameObject.Find("LevelSettings").GetComponent<LevelSettings>();
		if(ls.haveIntro){
			transform.localScale = new Vector3(25,25,25);
			StartCoroutine(Intro());
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
