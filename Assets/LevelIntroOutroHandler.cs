﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroOutroHandler : MonoBehaviour {
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
		villageNameText.text = lg.villageName;
		float t = 0.0f;
		while(t<textFadeInTime){
			t += Time.deltaTime;
			villageNameText.color = new Color(1,1,1,t/textFadeInTime);
			yield return null;
		}
		pic.transform.Translate(new Vector2(0,-1000));

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
		pic.transform.Translate(new Vector2(0,1000));
		t = 0.0f;
		while(t<fadeOutTime){
			t += Time.deltaTime;
			
			fadeInMask.color = new Color(0,0,0, 1 - (t/fadeOutTime)); 
			
			yield return null;
		}
		fadeInMask.color = new Color(0,0,0, 0); 
		yield return null;
	}

	IEnumerator Outro(){
		fadeInMask.color = new Color(0,0,0,0);
		float t = 0.0f;
		while(t<fadeOutTime){
			t += Time.deltaTime;
			
			fadeInMask.color = new Color(0,0,0,(t/fadeOutTime)); 
			
			yield return null;
		}
		fadeInMask.color = new Color(0,0,0,1); 
		yield return null;
		//change scene
	}

	public void LeaveLevel(){
		Debug.Log("leaving level");
		StartCoroutine(Outro());
	}

	void Start () {
		
		StartCoroutine(Intro());	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
