using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MealPakPanel : MonoBehaviour {
	Unit playerUnit;
	public Text numText;
	public Color enough;
	public Color notEnough;
	public Text notificationText;

	public string notificationString;

	float charDelay = .1f;
	float fadeTime = 2f;

	bool canNotify = false;
	// Use this for initialization
	void Start () {
		if(notificationText != null){
			notificationText.text = "";
		}
		playerUnit = GameObject.Find("PlayerInputController").GetComponent<PlayerInputController>().playerUnit;
		StartCoroutine(CheckPaksDelay());
		//notificationText.color = new Color(notificationText.color.r, notificationText.color.g,notificationText.color.b, 0);
	}

	IEnumerator CheckPaksDelay(){
		yield return new WaitForSeconds(1);
		if(playerUnit.GetMealPaks() < 3){
			canNotify = true;
		}
	}

	IEnumerator ShowText(){
		float t = fadeTime;
		for(int i = 0; i<notificationString.Length; i++){
			notificationText.text += notificationString[i];
			yield return new WaitForSeconds(charDelay);
		}

		while(t>0){
			notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, t / fadeTime);
			t -= Time.deltaTime;
			yield return null;
		}
		notificationText.color = new Color(notificationText.color.r, notificationText.color.g, notificationText.color.b, 0);

	}

	void NotifyPlayer(){
		StartCoroutine(ShowText());
	}
	
	// Update is called once per frame
	void Update () {
		if(playerUnit!=null){
			numText.text = playerUnit.GetMealPaks().ToString() + "x";
			if(playerUnit.GetMealPaks() > 2){
				if(canNotify){
					canNotify = false;
					NotifyPlayer();
				}
				numText.color = enough;	
			}else{
				numText.color = notEnough;
			}
		}
	}
}
